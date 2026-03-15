using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAuth.ACL.Interfaces;
using NNews.Domain.Services.Interfaces;
using zTools.ACL.Interfaces;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NNews.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUserClient _userClient;
        private readonly IFileClient _imageService;

        public ImageController(
            IUserClient userClient,
            IFileClient imageService
        )
        {
            _userClient = userClient;
            _imageService = imageService;
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost("uploadImage")]
        [Authorize]
        public async Task<ActionResult<string>> uploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var userSession = _userClient.GetUserInSession(HttpContext);
                if (userSession == null)
                {
                    return Unauthorized("Not Authorized");
                }

                var extension = Path.GetExtension(file.FileName) ?? ".jpg";
                var uniqueName = $"{Guid.NewGuid()}{extension}";
                var renamedFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, uniqueName)
                {
                    Headers = file.Headers,
                    ContentType = file.ContentType
                };

                var fileName = await _imageService.UploadFileAsync("NNews", renamedFile);
                var imageUrl = await _imageService.GetFileUrlAsync("NNews", fileName);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
