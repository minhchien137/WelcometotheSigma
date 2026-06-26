using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public interface IVipScheduleService
{
    Task<List<VipGuest>> GetVipGuestsAsync();
    Task<List<VipDisplaySchedule>> GetSchedulesAsync();
    Task<int> SaveScheduleAsync(SaveScheduleRequest req);
    Task DeleteScheduleAsync(int id);
    Task UpdateGuestImageAsync(int guestId, string imageFileName);
    Task<VipDisplaySchedule?> GetCurrentActiveAsync();
}
