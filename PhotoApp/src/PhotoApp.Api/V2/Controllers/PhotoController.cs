using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoApp.Core.Interfaces.Services;
using PhotoApp.Core.Models;

namespace PhotoApp.Api.V2.Controllers
{
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        /// <summary>
        /// Get all photos
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<PhotoModel>>> GetAll()
        {
            var response = await this._photoService.GetAll().ConfigureAwait(false);

            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
