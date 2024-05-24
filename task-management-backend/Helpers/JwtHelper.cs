using System.Text;

namespace task_management_backend.Helpers;

public static class JwtHelper
{
    public static byte[] GetKey()
    {
        var jwtKeyBase64 = Environment.GetEnvironmentVariable("JWT_KEY_BASE64");
        return jwtKeyBase64 == null ? "super-secret-jwt-key"u8.ToArray() : Convert.FromBase64String(jwtKeyBase64);
    }

    public static double GetExpireDays()
    {
        try
        {
            var jwtExpireDays = Environment.GetEnvironmentVariable("JWT_EXPIRE_DAYS");
            return string.IsNullOrWhiteSpace(jwtExpireDays) ? 1.0 : Convert.ToDouble(jwtExpireDays);
        }
        catch (Exception)
        {
            return 1.0;
        }
    }
    
    public static string GetIssuer()
    {
        return Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "task-management-backend";
    }

    public static string GetAudience()
    {
        return Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "task-management-frontend";
    }
}