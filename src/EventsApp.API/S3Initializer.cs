using Amazon.S3;

namespace EventsApp.API;

public static class S3Initializer
{
    /// <summary>
    /// Создание бакета если его еще нету 
    /// </summary>
    /// <param name="s3Client"></param>
    public static async Task EnsureBucketExistsAsync(AmazonS3Client s3Client)
    {
        const string bucketName = "event-pictures";
        try
        {
            await s3Client.GetBucketLocationAsync(bucketName);
        }
        catch (AmazonS3Exception e) when(e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await s3Client.PutBucketAsync(bucketName);
        }
    }
}