using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;
using Finsight.Client.AppFrame;
using Finsight.Client.Scenarios.Login;
using Finsight.Client.Scenarios.Main;
using Finsight.Contract.Dto;
using Flurl.Http;
using Unity;
using Container = Finsight.Client.DI.Container;

namespace Finsight.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly MainPresenter _mainPresenter;
    private readonly LoginPresenter _login;

    public App()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;
        FlurlHttp.Clients.Add("Client", ConfigurationManager.AppSettings.Get("apiUrl"));
        _mainPresenter = Container.Instance.Resolve<MainPresenter>();
        _login = Container.Instance.Resolve<LoginPresenter>();
        _login.OnLoginSuccess += _loginPresenterOnLoginSuccess;
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // Логирование ошибки
        HandleException(e.Exception);
        e.Handled = true; // Чтобы приложение не падало
    }

    private void HandleException(Exception ex)
    {
        var message = BuildErrorMessage(ex, out var stackTrace);
        var errorWindow = new ErrorView(message, stackTrace);
        errorWindow.ShowDialog();
    }

    private static string BuildErrorMessage(Exception ex, out string? stackTrace)
    {
        if (ex is null)
        {
            stackTrace = null;
            return string.Empty;
        }

        if (ex is AggregateException aggregate && aggregate.InnerExceptions.Count == 1)
        {
            return BuildErrorMessage(aggregate.InnerExceptions[0], out stackTrace);
        }

        if (ex is FlurlHttpException flurlException)
        {
            var problem = TryReadProblemDetails(flurlException);
            if (problem is not null)
            {
                stackTrace = BuildProblemDetailsDetails(flurlException, problem);
                return ComposeProblemDetailsMessage(problem, flurlException);
            }
        }

        if (ex.InnerException is not null)
        {
            return BuildErrorMessage(ex.InnerException, out stackTrace);
        }

        stackTrace = ex.StackTrace;
        return ex.Message;
    }

    private static string ComposeProblemDetailsMessage(ProblemDetailsDto problem, FlurlHttpException exception)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(problem.Title))
        {
            parts.Add(problem.Title);
        }

        if (!string.IsNullOrWhiteSpace(problem.Detail))
        {
            parts.Add(problem.Detail);
        }

        if (parts.Count == 0)
        {
            var status = problem.Status ?? exception.StatusCode;
            parts.Add(status is null
                ? "Произошла ошибка при обращении к серверу."
                : $"Произошла ошибка при обращении к серверу. Код состояния: {status}.");
        }

        return string.Join(Environment.NewLine + Environment.NewLine, parts);
    }

    private static string? BuildProblemDetailsDetails(FlurlHttpException exception, ProblemDetailsDto problem)
    {
        var details = new List<string>();

        var status = problem.Status ?? exception.StatusCode;
        if (status is not null)
        {
            details.Add($"Код состояния: {status}");
        }

        if (!string.IsNullOrWhiteSpace(problem.Type))
        {
            details.Add($"Тип: {problem.Type}");
        }

        if (!string.IsNullOrWhiteSpace(problem.Instance))
        {
            details.Add($"Экземпляр: {problem.Instance}");
        }

        if (details.Count == 0)
        {
            return exception.StackTrace;
        }

        return string.Join(Environment.NewLine, details);
    }

    private static ProblemDetailsDto? TryReadProblemDetails(FlurlHttpException exception)
    {
        try
        {
            return exception.GetResponseJsonAsync<ProblemDetailsDto>().GetAwaiter().GetResult();
        }
        catch
        {
            return null;
        }
    }

    private async void _loginPresenterOnLoginSuccess(object? sender, UserDto user)
    {
        _login.CloseView();
        await _mainPresenter.ShowAsync();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _login.StartApplication();
    }
}
