namespace WelcometotheSigma.Models;

public class VipQueueItem
{
    public int Id { get; set; }
    public string EmpCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public string? PhotoBase64 { get; set; }
    public DateTime CheckDatetime { get; set; }
    public string? Area { get; set; }
    public bool IsDisplay { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class VipEmployee
{
    public string EmpCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Photo { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}
