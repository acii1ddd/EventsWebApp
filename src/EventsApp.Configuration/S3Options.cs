namespace EventsApp.Configuration;

public class S3Options
{
    public string Region { get; set; } = string.Empty;
    
    public string ServiceUrl { get; set; } = string.Empty;
    
    public bool ForcePathStyle { get; set; } = false;
    
    public bool UseHttp { get; set; }

    public string AccessKey { get; set; } = string.Empty;
    
    public string SecretKey { get; set; } = string.Empty;
}