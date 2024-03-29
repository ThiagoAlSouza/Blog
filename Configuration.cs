﻿namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "JhsG62JHSou3qn8hKSLos6FAV";
    public static string ApiKeyName { get; set; } = "api_key";
    public static string ApiKey { get; set; } = "SrvG623HS2u3q3ahKSLpl6F1V";

    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; } = 25;
        public string UserName { get; set; }    
        public string Password { get; set; }
    }
}