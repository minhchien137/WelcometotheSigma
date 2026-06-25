using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public interface IVipQueueService
{
    Task<VipQueueItem?> GetNextPendingAsync();
    Task MarkAsDisplayedAsync(int id);
    Task<List<VipEmployee>> GetVipEmployeesAsync();
}
