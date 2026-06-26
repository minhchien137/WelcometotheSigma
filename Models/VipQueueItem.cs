namespace WelcometotheSigma.Models;

public class VipGuest
{
    public int Id { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string? ImageFileName { get; set; }
}

public class VipDisplaySchedule
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string? ImageFileName { get; set; }
    public string StartTime { get; set; } = "08:00";
    public string EndTime { get; set; } = "09:00";
    public bool IsActive { get; set; } = true;
}

public class SaveScheduleRequest
{
    public int? Id { get; set; }
    public int GuestId { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}
