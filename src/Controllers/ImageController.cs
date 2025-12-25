using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {

        private const string S3BucketName = "uploads-app-bucket";
        private readonly IAmazonS3 _s3Client;
        public ImageController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadVM vm)
        {

            if (vm.File.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = vm.File.OpenReadStream();
            var key = Guid.NewGuid();
            var putRequest = new PutObjectRequest
            {
                BucketName = S3BucketName,
                Key = $"images/{key}",
                InputStream = stream,
                ContentType = vm.File.ContentType
            };

            await _s3Client.PutObjectAsync(putRequest);
            return Ok(new { Message = "File uploaded to S3!", S3Key = key });

        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string key)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = S3BucketName,
                Key = $"images/{key}",
            };
            var response = await _s3Client.GetObjectAsync(getRequest);
            return File(response.ResponseStream, response.Headers.ContentType);
        }
    }
}



