namespace WelcometotheSigma.Models;

public class DisplayConfig
{
    public string IdleTitle       { get; set; } = "Hệ thống chào mừng";
    public string IdleSubtitle    { get; set; } = "VIP Welcome System";
    public int    IdleTitleSizePx { get; set; } = 22;
    public string BackgroundColor { get; set; } = "#ffffff";
    public string DividerColor    { get; set; } = "#C8102E";
    public int    PollIntervalSeconds { get; set; } = 5;
}
