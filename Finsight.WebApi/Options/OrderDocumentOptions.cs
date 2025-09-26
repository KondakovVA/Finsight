using System.ComponentModel.DataAnnotations;

namespace Finsight.WebApi.Options
{
    public sealed class OrderDocumentOptions
    {
        public const string SectionName = "OrderDocuments";

        [Required]
        public string RootPath { get; set; } = "OrderDocuments";

        public HashSet<string> AllowedExtensions { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".xls",
            ".xlsx",
            ".doc",
            ".docx"
        };
    }
}
