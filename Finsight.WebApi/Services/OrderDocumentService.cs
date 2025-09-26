using Finsight.Core.Exceptions;
using Finsight.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Finsight.WebApi.Services
{
    public sealed class OrderDocumentService : IOrderDocumentService
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        private readonly IWebHostEnvironment _environment;
        private readonly IOptionsMonitor<OrderDocumentOptions> _optionsMonitor;
        private readonly ILogger<OrderDocumentService> _logger;

        public OrderDocumentService(
            IWebHostEnvironment environment,
            IOptionsMonitor<OrderDocumentOptions> optionsMonitor,
            ILogger<OrderDocumentService> logger)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> SaveAsync(string username, IReadOnlyCollection<IFormFile> files, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ValidationException("Не удалось определить пользователя.");
            }

            var sanitizedUsername = new string(username.Where(ch => !InvalidFileNameChars.Contains(ch)).ToArray());
            if (string.IsNullOrWhiteSpace(sanitizedUsername))
            {
                throw new ValidationException("Не удалось определить пользователя.");
            }

            if (files == null || files.Count == 0)
            {
                throw new ValidationException("Не загружен ни один файл.");
            }

            var validFiles = files.Where(f => f != null && f.Length > 0).ToList();
            if (validFiles.Count == 0)
            {
                throw new ValidationException("Не загружен ни один файл.");
            }

            var options = _optionsMonitor.CurrentValue;
            var allowedExtensions = options.AllowedExtensions;
            var rootPath = Path.Combine(_environment.ContentRootPath, options.RootPath);
            Directory.CreateDirectory(rootPath);

            var relativeFolder = Path.Combine(sanitizedUsername, Guid.NewGuid().ToString("N"));
            var targetDirectory = Path.Combine(rootPath, relativeFolder);
            Directory.CreateDirectory(targetDirectory);

            foreach (var file in validFiles)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    throw new ValidationException($"Формат файла '{extension}' не поддерживается.");
                }

                var fileName = Path.GetFileName(file.FileName);
                var destinationPath = Path.Combine(targetDirectory, fileName);

                await using var stream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await file.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Документ {FileName} загружен для пользователя {User}.", fileName, sanitizedUsername);
            }

            var normalizedPath = relativeFolder.Replace(Path.DirectorySeparatorChar, '/');
            _logger.LogInformation("Сохранено {FileCount} документов для пользователя {User}.", validFiles.Count, sanitizedUsername);
            return normalizedPath;
        }
    }
}
