namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "JhsG62JHSou3qn8hKSLos6FAV";
    public static string ApiKeyName { get; set; } = "api_key";
    public static string ApiKey { get; set; } = "SrvG623HS2u3q3ahKSLpl6F1V";

    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public static string Host { get; set; }
        public static int Port { get; set; } = 25;
        public static string Email { get; set; }
        public static string Password { get; set; }
    }
}
