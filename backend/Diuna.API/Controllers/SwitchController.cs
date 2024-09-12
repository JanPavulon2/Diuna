using Diuna.Models.Config;
using Diuna.Models.State;
using Diuna.Services.Switch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Diuna.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SwitchController : ControllerBase
{
    private readonly ISwitchService _switchService;

    public SwitchController(ISwitchService switchService)
    {
        _switchService = switchService;
    }

    [HttpGet]
    public IEnumerable<SwitchConfig> GetSwitches()
    {
        return _switchService.GetAllSwitches();
    }

    [HttpGet("{tag}")]
    public ActionResult<SwitchConfig> GetSwitch(string tag)
    {
        var switchControl = _switchService.GetSwitchByTag(tag);
        if (switchControl == null) 
            return NotFound();

        return Ok(switchControl);
    }

    [HttpPost("{tag}/setSwitchState/{on}")]
    public IActionResult SetSwitchState(string tag, bool on)
    {
        _switchService.SetSwitchAsync(tag, on);
        return NoContent();
    }


    [HttpPost("{tag}/toggle")]
    public IActionResult ToggleSwitch(string tag)
    {
        _switchService.ToggleSwitchAsync(tag);
        return NoContent();
    }

    [HttpPost("{tag}/on")]
    public IActionResult TurnOnSwitch(string tag)
    {
        _switchService.TurnOnSwitch(tag);
        return NoContent();
    }

    [HttpPost("{tag}/off")]
    public IActionResult TurnOffSwitch(string tag)
    {
        _switchService.TurnOffSwitch(tag);
        return NoContent();
    }
}
