using Microsoft.AspNetCore.Mvc;
using WelcometotheSigma.Models;
using WelcometotheSigma.Services;

namespace WelcometotheSigma.Controllers;

public class AdminController : Controller
{
    private readonly IDisplayConfigService _cfgService;
    private readonly IVipScheduleService   _scheduleService;
    private readonly IWebHostEnvironment   _env;

    public AdminController(IDisplayConfigService cfgService, IVipScheduleService scheduleService, IWebHostEnvironment env)
    {
        _cfgService      = cfgService;
        _scheduleService = scheduleService;
        _env             = env;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Config() => Json(_cfgService.Get());

    [HttpPost]
    public IActionResult SaveConfig([FromBody] DisplayConfig config)
    {
        if (config is null)
            return Json(new { success = false, message = "Invalid payload" });
        try
        {
            _cfgService.Save(config);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetVipGuests()
    {
        try { return Json(await _scheduleService.GetVipGuestsAsync()); }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    [HttpGet]
    public async Task<IActionResult> GetSchedules()
    {
        try { return Json(await _scheduleService.GetSchedulesAsync()); }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> SaveSchedule([FromBody] SaveScheduleRequest req)
    {
        if (req == null || req.GuestId <= 0 || string.IsNullOrEmpty(req.StartTime) || string.IsNullOrEmpty(req.EndTime))
            return Json(new { success = false, message = "Thiếu dữ liệu" });
        try
        {
            var id = await _scheduleService.SaveScheduleAsync(req);
            return Json(new { success = true, id });
        }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        try
        {
            await _scheduleService.DeleteScheduleAsync(id);
            return Json(new { success = true });
        }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> UploadWelcomeImage([FromForm] int guestId, IFormFile photo)
    {
        if (guestId <= 0 || photo == null || photo.Length == 0)
            return Json(new { success = false, message = "Thiếu dữ liệu" });
        try
        {
            var guests = await _scheduleService.GetVipGuestsAsync();
            var guest  = guests.FirstOrDefault(g => g.Id == guestId);
            if (guest == null)
                return Json(new { success = false, message = "Không tìm thấy khách VIP" });

            var safeName = new string(guest.GuestName
                .Select(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' ? c : '_')
                .ToArray()).Trim('_');
            var ext      = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext)) ext = ".jpg";
            var fileName = $"{safeName}{ext}";
            var savePath = Path.Combine(_env.WebRootPath, "images", fileName);

            await using var stream = new FileStream(savePath, FileMode.Create);
            await photo.CopyToAsync(stream);

            await _scheduleService.UpdateGuestImageAsync(guestId, fileName);

            return Json(new { success = true, message = $"Đã lưu ảnh: {fileName}", fileName });
        }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }
}
