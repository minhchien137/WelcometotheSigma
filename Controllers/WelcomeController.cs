using Microsoft.AspNetCore.Mvc;
using WelcometotheSigma.Models;
using WelcometotheSigma.Services;

namespace WelcometotheSigma.Controllers;

public class WelcomeController : Controller
{
    private readonly IVipQueueService _vipQueueService;
    private readonly ILogger<WelcomeController> _logger;

    public WelcomeController(IVipQueueService vipQueueService, ILogger<WelcomeController> logger)
    {
        _vipQueueService = vipQueueService;
        _logger = logger;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> Next()
    {
        try
        {
            var item = await _vipQueueService.GetNextPendingAsync();
            return Json(new ApiResponse<VipQueueItem>
            {
                Success = true,
                Data    = item
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching next VIP");
            return Json(new ApiResponse<VipQueueItem>
            {
                Success = false,
                Message = "Error fetching next VIP"
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Displayed(int id)
    {
        try
        {
            await _vipQueueService.MarkAsDisplayedAsync(id);
            return Json(new ApiResponse<object> { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking VIP {Id} as displayed", id);
            return Json(new ApiResponse<object>
            {
                Success = false,
                Message = "Error updating display status"
            });
        }
    }
}
