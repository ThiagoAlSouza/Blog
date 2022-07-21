namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "JhsG62JHSou3qn8hKSLos6FAV";
    public static string ApiKeyName { get; set; } = "api_key";
    public static string ApiKey { get; set; } = "SrvG623HS2u3q3ahKSLpl6F1V";

    public static SmtpConfiguration Smtp = new();

    //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
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
