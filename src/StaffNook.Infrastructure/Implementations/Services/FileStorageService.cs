using Amazon.S3;
using Amazon.S3.Model;
using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos.Attachments;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Converters;

namespace StaffNook.Infrastructure.Implementations.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IAttachmentsService _attachmentsService;

    public FileStorageService(
        IAttachmentsService attachmentsService)
    {
        _attachmentsService = attachmentsService;
    }

    public async Task<string> GetPreviewUrl(Guid attachmentId)
    {
        var attachment = await _attachmentsService.GetById(attachmentId);
        return GetPreviewUrlString(attachment);
    }

    private static string GetPreviewUrlString(AttachmentDto attachment) =>
        $"{AppConfiguration.FileStorage.PreviewUrl}/{attachment.Bucket.ToString().ToLower()}/{attachment.Path}/{attachment.UniqueFileName}";

    public async Task<FileDto> GetFileDto(Guid attachmentId)
    {
        var attachment = await _attachmentsService.GetById(attachmentId);
        
        return new FileDto()
        {
            Id = attachment.Id,
            Bucket = attachment.Bucket,
            BucketName = FileStorageConverter.GetBucketName(attachment.Bucket),
            Path = attachment.Path,
            Name = attachment.UniqueFileName,
            PreviewUrl = GetPreviewUrlString(attachment)
        };
    }

    public async Task<FileDto> Store(string filePath, Stream content, string fileName, FileStorageBucket bucket)
    {
        var fileKey = IFileStorageService.RandomizeFileName(fileName);
        var bucketName = FileStorageConverter.GetBucketName(bucket);
        using var client = GetClient();
        try
        {
            var putObject = new PutObjectRequest
            {
                Key = $"{filePath}/{fileKey}",
                InputStream = content,
                BucketName = bucketName,
                Headers = {ContentLength = content.Length},
            };
            await client.PutObjectAsync(putObject);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        var fileId = await _attachmentsService.Create(new AttachmentCreateDto
        {
            UniqueFileName = fileKey,
            Path = filePath,
            Bucket = bucket,
        });

        return new FileDto()
        {
            Id = fileId,
            Bucket = bucket,
            BucketName = FileStorageConverter.GetBucketName(bucket),
            Path = filePath,
            Name = fileKey,
            PreviewUrl = await GetPreviewUrl(fileId)
        };
    }

    public async Task<FileDto> Store(string filePath, byte[] content, string fileName, FileStorageBucket bucket)
    {
        await using var memoryStream = new MemoryStream(content);
        return await Store(filePath, memoryStream, fileName, bucket);
    }

    public async Task Delete(string filePath, string fileKey, FileStorageBucket bucket)
    {
        var bucketName = FileStorageConverter.GetBucketName(bucket);
        using var client = GetClient();
        try
        {
            await client.DeleteObjectAsync(bucketName, $"{filePath}/{fileKey}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task DeleteByAttachmentId(Guid attachmentId)
    {
        var attachment = await _attachmentsService.GetById(attachmentId);
        await Delete(attachment.Path, attachment.UniqueFileName, attachment.Bucket);
        await _attachmentsService.Delete(attachmentId);
    }

    public async Task TempDeleteByAttachmentId(Guid attachmentId)
    {
        var attachment = await _attachmentsService.GetById(attachmentId);
        if (attachment.Bucket != FileStorageBucket.Temp)
        {
            await MoveFileByAttachmentId(attachment.Id, FileStorageBucket.Temp);
        }
    }

    public async Task<FileDto> MoveFileByAttachmentId(Guid attachmentId, FileStorageBucket destinationBucket, string newFileName = null)
    {
        var attachment = await _attachmentsService.GetById(attachmentId);
        var fileDto = await MoveFile(attachment.Path, attachment.UniqueFileName, attachment.Bucket, destinationBucket, newFileName);
        attachment.Bucket = destinationBucket;
        if (newFileName is not null)
        {
            attachment.UniqueFileName = newFileName;
        }

        await _attachmentsService.Update(attachment);

        return fileDto;
    }

    public async Task<FileDto> MoveFile(string filePath, string fileKey, FileStorageBucket sourceBucket, FileStorageBucket destinationBucket,
        string newFileName = null)
    {
        var sourceBucketName = FileStorageConverter.GetBucketName(sourceBucket);
        var destinationBucketName = FileStorageConverter.GetBucketName(destinationBucket);
        newFileName = string.IsNullOrEmpty(newFileName) ? fileKey : newFileName;
        var oldFilePath = $"{filePath}/{fileKey}";
        var newFilePath = $"{filePath}/{newFileName}";
        using var client = GetClient();
        try
        {
            await client.CopyObjectAsync(sourceBucketName, oldFilePath, destinationBucketName, newFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        try
        {
            await client.DeleteObjectAsync(sourceBucketName, oldFilePath);
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        
        return new FileDto
        {
            Bucket = destinationBucket,
            BucketName = FileStorageConverter.GetBucketName(destinationBucket),
            Name = newFileName,
            Path = filePath,
            PreviewUrl = AppConfiguration.FileStorage.PreviewUrl // todo??
        };
    }
    
    private static AmazonS3Client GetClient()
    {
        return new AmazonS3Client(
            awsAccessKeyId:AppConfiguration.FileStorage.AccessKey,
            awsSecretAccessKey:AppConfiguration.FileStorage.SecretKey,
            clientConfig: new AmazonS3Config
            {
                ServiceURL = AppConfiguration.FileStorage.Endpoint,
                ForcePathStyle = AppConfiguration.FileStorage.ForcePathStyle
            });
    }
}