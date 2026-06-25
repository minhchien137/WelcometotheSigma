using Microsoft.AspNetCore.Mvc;
using WelcometotheSigma.Models;
using WelcometotheSigma.Services;

namespace WelcometotheSigma.Controllers;

public class AdminController : Controller
{
    private readonly IDisplayConfigService _cfgService;
    private readonly IVipQueueService _vipQueueService;
    private readonly IWebHostEnvironment _env;

    public AdminController(IDisplayConfigService cfgService, IVipQueueService vipQueueService, IWebHostEnvironment env)
    {
        _cfgService = cfgService;
        _vipQueueService = vipQueueService;
        _env = env;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Config() => Json(_cfgService.Get());

    [HttpPost]
    public IActionResult SaveConfig([FromBody] DisplayConfig config)
    {
        if (config is null)
            return BadRequest(new { success = false, message = "Invalid payload" });

        try
        {
            _cfgService.Save(config);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetVipEmployees()
    {
        try
        {
            var list = await _vipQueueService.GetVipEmployeesAsync();
            return Json(list);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadVipPhoto([FromForm] string empCode, IFormFile photo)
    {
        if (string.IsNullOrWhiteSpace(empCode) || photo == null || photo.Length == 0)
            return BadRequest(new { success = false, message = "Thiếu dữ liệu" });

        try
        {
            var employees = await _vipQueueService.GetVipEmployeesAsync();
            var emp = employees.FirstOrDefault(e => e.EmpCode == empCode);
            if (emp == null)
                return NotFound(new { success = false, message = "Không tìm thấy nhân viên" });
            if (string.IsNullOrEmpty(emp.Photo))
                return BadRequest(new { success = false, message = "Nhân viên chưa có tên file ảnh trong database (cột photo trống)" });

            var fileName = Path.GetFileName(emp.Photo);
            var savePath = Path.Combine(_env.WebRootPath, "images", fileName);

            await using var stream = new FileStream(savePath, FileMode.Create);
            await photo.CopyToAsync(stream);

            return Json(new { success = true, message = $"Đã lưu ảnh: {fileName}", fileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
