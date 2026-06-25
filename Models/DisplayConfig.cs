namespace WelcometotheSigma.Models;

public class DisplayConfig
{
    // Text content
    public string WelcomeText       { get; set; } = "✦  Kính chào Quý Khách  ✦";
    public string IdleTitle         { get; set; } = "Hệ thống chào mừng";
    public string IdleSubtitle      { get; set; } = "VIP Welcome System";
    public string BrandName         { get; set; } = "Sigma";
    public string BrandSubtitle     { get; set; } = "VIP Welcome";

    // Colors
    public string BackgroundColor     { get; set; } = "#ffffff";
    public string CornerBorderColor   { get; set; } = "#C8102E";
    public string WelcomeTextColor    { get; set; } = "#C8102E";
    public string GuestNameColor      { get; set; } = "#1a1a1a";
    public string GuestNameAccentColor{ get; set; } = "#C8102E";
    public string AvatarRingColor     { get; set; } = "#C8102E";
    public string DividerColor        { get; set; } = "#C8102E";

    // Font sizes (px)
    public int WelcomeTextSizePx { get; set; } = 32;
    public int GuestNameSizePx   { get; set; } = 56;
    public int IdleTitleSizePx   { get; set; } = 22;

    // Timing
    public int DisplayDurationSeconds { get; set; } = 9;
    public int PollIntervalSeconds    { get; set; } = 3;

    // Effects
    public bool   ShowFireworks       { get; set; } = true;
    public string FireworksIntensity  { get; set; } = "medium"; // low | medium | high

    // Positions — pixel offset from default centered position
    public int GuestContentOffsetY { get; set; } = 0;   // toàn bộ khối khách
    public int IdleContentOffsetY  { get; set; } = 0;   // toàn bộ khối idle
    public int WelcomeTextOffsetX  { get; set; } = 0;
    public int WelcomeTextOffsetY  { get; set; } = 0;
    public int AvatarOffsetX       { get; set; } = 0;
    public int AvatarOffsetY       { get; set; } = 0;
    public int GuestNameOffsetX    { get; set; } = 0;
    public int GuestNameOffsetY    { get; set; } = 0;
}
