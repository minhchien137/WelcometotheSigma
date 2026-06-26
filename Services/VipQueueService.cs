using Microsoft.Data.SqlClient;
using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public class VipScheduleService : IVipScheduleService
{
    private readonly string _conn;
    private readonly ILogger<VipScheduleService> _logger;

    public VipScheduleService(IConfiguration configuration, ILogger<VipScheduleService> logger)
    {
        _conn = configuration.GetConnectionString("ZkTecoDb")
            ?? throw new InvalidOperationException("Connection string 'ZkTecoDb' not found.");
        _logger = logger;
    }

    public async Task<List<VipGuest>> GetVipGuestsAsync()
    {
        var result = new List<VipGuest>();
        await using var con = new SqlConnection(_conn);
        await con.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT Id, GuestName, ImageFileName FROM VipGuest ORDER BY GuestName", con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            result.Add(new VipGuest
            {
                Id            = r.GetInt32(0),
                GuestName     = r.GetString(1),
                ImageFileName = r.IsDBNull(2) ? null : r.GetString(2)
            });
        return result;
    }

    public async Task<List<VipDisplaySchedule>> GetSchedulesAsync()
    {
        var result = new List<VipDisplaySchedule>();
        await using var con = new SqlConnection(_conn);
        await con.OpenAsync();
        const string sql = @"
            SELECT s.Id, s.GuestId, g.GuestName, g.ImageFileName,
                   CONVERT(varchar(5), s.StartTime, 108) AS StartTime,
                   CONVERT(varchar(5), s.EndTime,   108) AS EndTime,
                   s.IsActive
            FROM VipDisplaySchedule s
            JOIN VipGuest g ON g.Id = s.GuestId
            ORDER BY s.StartTime";
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            result.Add(new VipDisplaySchedule
            {
                Id            = r.GetInt32(0),
                GuestId       = r.GetInt32(1),
                GuestName     = r.GetString(2),
                ImageFileName = r.IsDBNull(3) ? null : r.GetString(3),
                StartTime     = r.GetString(4),
                EndTime       = r.GetString(5),
                IsActive      = r.GetBoolean(6)
            });
        return result;
    }

    public async Task<int> SaveScheduleAsync(SaveScheduleRequest req)
    {
        await using var con = new SqlConnection(_conn);
        await con.OpenAsync();

        if (req.Id.HasValue && req.Id > 0)
        {
            const string sql = @"
                UPDATE VipDisplaySchedule
                SET GuestId = @GuestId, StartTime = @StartTime, EndTime = @EndTime, IsActive = @IsActive
                WHERE Id = @Id";
            await using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id",        req.Id.Value);
            cmd.Parameters.AddWithValue("@GuestId",   req.GuestId);
            cmd.Parameters.AddWithValue("@StartTime", req.StartTime);
            cmd.Parameters.AddWithValue("@EndTime",   req.EndTime);
            cmd.Parameters.AddWithValue("@IsActive",  req.IsActive);
            await cmd.ExecuteNonQueryAsync();
            return req.Id.Value;
        }
        else
        {
            const string sql = @"
                INSERT INTO VipDisplaySchedule (GuestId, StartTime, EndTime, IsActive)
                VALUES (@GuestId, @StartTime, @EndTime, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            await using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@GuestId",   req.GuestId);
            cmd.Parameters.AddWithValue("@StartTime", req.StartTime);
            cmd.Parameters.AddWithValue("@EndTime",   req.EndTime);
            cmd.Parameters.AddWithValue("@IsActive",  req.IsActive);
            var newId = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(newId);
        }
    }

    public async Task DeleteScheduleAsync(int id)
    {
        await using var con = new SqlConnection(_conn);
        await con.OpenAsync();
        await using var cmd = new SqlCommand(
            "DELETE FROM VipDisplaySchedule WHERE Id = @Id", con);
        cmd.Parameters.AddWithValue("@Id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateGuestImageAsync(int guestId, string imageFileName)
    {
        await using var con = new SqlConnection(_conn);
        await con.OpenAsync();
        await using var cmd = new SqlCommand(
            "UPDATE VipGuest SET ImageFileName = @ImageFileName WHERE Id = @Id", con);
        cmd.Parameters.AddWithValue("@Id",            guestId);
        cmd.Parameters.AddWithValue("@ImageFileName", imageFileName);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<VipDisplaySchedule?> GetCurrentActiveAsync()
    {
        var now = DateTime.Now.ToString("HH:mm:ss");
        await using var con = new SqlConnection(_conn);
        await con.OpenAsync();
        const string sql = @"
            SELECT TOP 1 s.Id, s.GuestId, g.GuestName, g.ImageFileName,
                   CONVERT(varchar(5), s.StartTime, 108) AS StartTime,
                   CONVERT(varchar(5), s.EndTime,   108) AS EndTime,
                   s.IsActive
            FROM VipDisplaySchedule s
            JOIN VipGuest g ON g.Id = s.GuestId
            WHERE s.IsActive = 1
              AND s.StartTime <= CAST(@Now AS TIME)
              AND s.EndTime   >= CAST(@Now AS TIME)
            ORDER BY s.StartTime";
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Now", now);
        await using var r = await cmd.ExecuteReaderAsync();
        if (!await r.ReadAsync()) return null;
        return new VipDisplaySchedule
        {
            Id            = r.GetInt32(0),
            GuestId       = r.GetInt32(1),
            GuestName     = r.GetString(2),
            ImageFileName = r.IsDBNull(3) ? null : r.GetString(3),
            StartTime     = r.GetString(4),
            EndTime       = r.GetString(5),
            IsActive      = r.GetBoolean(6)
        };
    }
}
