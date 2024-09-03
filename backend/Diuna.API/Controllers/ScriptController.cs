using Diuna.Services;
using Microsoft.AspNetCore.Mvc;

namespace Diuna.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScriptController : ControllerBase
{
    private readonly IScriptService _scriptService;

    public ScriptController(IScriptService scriptService)
    {
        _scriptService = scriptService;
    }

    [HttpPost("run")]
    public IActionResult RunScript([FromBody] ScriptRequest request)
    {
        try
        {
            string result = _scriptService.RunScript(request.ScriptPath);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

public class ScriptRequest
{
    public string ScriptPath { get; set; }
}
