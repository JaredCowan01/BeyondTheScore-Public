using BeyondTheScore.ApiService.Interfaces;
using BeyondTheScore.ApiService.Models.Import;
using Microsoft.AspNetCore.Mvc;

namespace BeyondTheScore.ApiService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly IMatchImportService _importer;

    public ImportController(IMatchImportService importer)
    {
        _importer = importer;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest();
        }
        
        ImportResult result = await _importer.ImportAsync(file.OpenReadStream());

        return Ok(result);
    }
}