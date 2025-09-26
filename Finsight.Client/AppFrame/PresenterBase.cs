using System.Windows.Controls;

namespace Finsight.Client.AppFrame
{
    public interface IPresenter
    {
        ContentControl ViewContent { get; }

        string ViewTitle { get; }

        Task ActivateAsync();
    }

    public abstract class PresenterBase<TView, TViewModel> : IPresenter
        where TView : ContentControl
        where TViewModel : ViewModelBase
    {
        private readonly TView _view;
        private readonly TViewModel _viewModel;
        private readonly string _viewTitle;
        private bool _isActivated;

        protected PresenterBase(TView view, TViewModel? viewModel = null, string? viewTitle = null)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            if (viewModel != null)
            {
                _view.DataContext = viewModel;
            }

            var resolvedViewModel = viewModel ?? _view.DataContext as TViewModel;
            if (resolvedViewModel is null)
            {
                throw new ArgumentException($"Представление должно иметь контекст данных типа {typeof(TViewModel)}.", nameof(view));
            }

            _viewModel = resolvedViewModel;

            if (!ReferenceEquals(_view.DataContext, _viewModel))
            {
                _view.DataContext = _viewModel;
            }

            _viewTitle = !string.IsNullOrWhiteSpace(viewTitle)
                ? viewTitle
                : _viewModel.ViewTitle ?? string.Empty;
        }

        protected TView View => _view;

        protected TViewModel ViewModel => _viewModel;

        public virtual ContentControl ViewContent => _view;

        public virtual string ViewTitle => _viewTitle;

        public Task ActivateAsync()
        {
            var activationTask = OnActivateAsync(!_isActivated);
            _isActivated = true;
            return activationTask ?? Task.CompletedTask;
        }

        protected virtual Task OnActivateAsync(bool isInitialActivation)
        {
            return Task.CompletedTask;
        }
    }
}
