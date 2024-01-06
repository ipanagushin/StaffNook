using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Common;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

// todo rework
[Route("api/v1/storage")]
public class AttachmentController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    public AttachmentController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    [HttpGet("preview/{attachmentId:guid}")]
    public async Task<IActionResult> GetPreviewUrlByAttachmentId(Guid attachmentId)
    {
        return Ok(await _fileStorageService.GetPreviewUrl(attachmentId));
    }
    
    [HttpGet("{attachmentId:guid}")]
    public async Task<IActionResult> GetFileDtoByAttachmentId(Guid attachmentId)
    {
        return Ok(await _fileStorageService.GetFileDto(attachmentId));
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromQuery] string filePath, [FromQuery] FileStorageBucket fileStorageBucket, [FromForm]IFormFile file)
    {
        try
        {
            if (file is null)
            {
                return BadRequest("Файл не прикреплен");
            }
            
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            
            var result = await _fileStorageService.Store(filePath, stream, file.FileName, fileStorageBucket);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            // for test
            return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка при загрузке файла");
        }
    }

    [HttpDelete("{id:guid}/delete")]
    public async Task<IActionResult> DeleteFile(Guid id, bool soft)
    {
        if (soft)
        {
            await _fileStorageService.TempDeleteByAttachmentId(id);
        }
        else
        {
            await _fileStorageService.DeleteByAttachmentId(id);
        }
        
        return NoContent();
    }
}