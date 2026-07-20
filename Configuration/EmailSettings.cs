namespace ServicioSoftware.API.Configuration;

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
}