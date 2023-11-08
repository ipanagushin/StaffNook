namespace StaffNook.Domain.Configuration;

public class FileStorageConfiguration
{
    public string Endpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    
    /// <summary>
    /// При использовании minio должен быть true
    /// </summary>
    public bool ForcePathStyle { get; set; }
    public string TempBucket { get; set; }
    public string PermanentBucket { get; set; }
    
    /// <summary>
    /// Тут может быть кастомный адрес в случае проксирования запросов на minio через nginx
    /// </summary>
    public string PreviewUrl { get; set; }
}