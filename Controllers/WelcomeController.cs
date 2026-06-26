using Microsoft.AspNetCore.Mvc;
using WelcometotheSigma.Services;

namespace WelcometotheSigma.Controllers;

public class WelcomeController : Controller
{
    private readonly IVipDisplayStateService _displayState;

    public WelcomeController(IVipDisplayStateService displayState)
    {
        _displayState = displayState;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult GetCurrentDisplay()
    {
        var current = _displayState.Current;
        if (current == null || string.IsNullOrEmpty(current.ImageFileName))
            return Json(new { imagePath = (string?)null });
        return Json(new { imagePath = $"/images/{current.ImageFileName}" });
    }
}
