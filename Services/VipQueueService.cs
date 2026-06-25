using Microsoft.Data.SqlClient;
using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public class VipQueueService : IVipQueueService
{
    private readonly string _connectionString;
    private readonly ILogger<VipQueueService> _logger;

    public VipQueueService(IConfiguration configuration, ILogger<VipQueueService> logger)
    {
        _connectionString = configuration.GetConnectionString("ZkTecoDb")
            ?? throw new InvalidOperationException("Connection string 'ZkTecoDb' not found.");
        _logger = logger;
    }

    public async Task<VipQueueItem?> GetNextPendingAsync()
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("sp_GetNextVipToDisplay", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            var item = new VipQueueItem
            {
                Id           = reader.GetInt32(reader.GetOrdinal("id")),
                EmpCode      = reader.IsDBNull(reader.GetOrdinal("emp_code"))   ? string.Empty : reader.GetString(reader.GetOrdinal("emp_code")),
                DisplayName  = reader.IsDBNull(reader.GetOrdinal("display_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("display_name")),
                PhotoPath    = reader.IsDBNull(reader.GetOrdinal("photo_path")) ? null : reader.GetString(reader.GetOrdinal("photo_path")),
                CheckDatetime = reader.GetDateTime(reader.GetOrdinal("check_datetime")),
                Area         = reader.IsDBNull(reader.GetOrdinal("area"))       ? null : reader.GetString(reader.GetOrdinal("area")),
                IsDisplay    = reader.GetBoolean(reader.GetOrdinal("is_display")),
                CreatedAt    = reader.GetDateTime(reader.GetOrdinal("created_at")),
            };

            if (!string.IsNullOrEmpty(item.PhotoPath))
            {
                var fileName = Path.GetFileName(item.PhotoPath);
                item.PhotoBase64 = $"/images/{fileName}";
            }

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetNextPendingAsync");
            throw;
        }
    }

    public async Task MarkAsDisplayedAsync(int id)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("sp_MarkVipAsDisplayed", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in MarkAsDisplayedAsync for id={Id}", id);
            throw;
        }
    }

    public async Task<List<VipEmployee>> GetVipEmployeesAsync()
    {
        try
        {
            var result = new List<VipEmployee>();
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("sp_GetVipEmployees", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new VipEmployee
                {
                    EmpCode     = reader.IsDBNull(reader.GetOrdinal("emp_code"))     ? string.Empty : reader.GetString(reader.GetOrdinal("emp_code")),
                    DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("display_name")),
                    Photo       = reader.IsDBNull(reader.GetOrdinal("photo"))        ? null         : reader.GetString(reader.GetOrdinal("photo")),
                });
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetVipEmployeesAsync");
            throw;
        }
    }
}
