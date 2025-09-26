namespace Finsight.WebApi.Services
{
    public interface IOrderDocumentService
    {
        Task<string> SaveAsync(string username, IReadOnlyCollection<IFormFile> files, CancellationToken cancellationToken);
    }
}
