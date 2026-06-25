using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public interface IDisplayConfigService
{
    DisplayConfig Get();
    void Save(DisplayConfig config);
}
