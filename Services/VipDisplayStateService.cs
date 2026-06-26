using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public interface IVipDisplayStateService
{
    VipDisplaySchedule? Current { get; set; }
}

public class VipDisplayStateService : IVipDisplayStateService
{
    private VipDisplaySchedule? _current;
    private readonly object _lock = new();

    public VipDisplaySchedule? Current
    {
        get { lock (_lock) return _current; }
        set { lock (_lock) _current = value; }
    }
}
