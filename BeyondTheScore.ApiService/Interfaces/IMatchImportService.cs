using BeyondTheScore.ApiService.Models.Import;

namespace BeyondTheScore.ApiService.Interfaces;

public interface IMatchImportService
{
    Task<ImportResult> ImportAsync(Stream csvStream);
}