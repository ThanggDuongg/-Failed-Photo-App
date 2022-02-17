using Microsoft.Extensions.Logging;
using PhotoApp.Core.Interfaces.Repositories;
using PhotoApp.Core.Interfaces.Services;
using PhotoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Core.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly ILogger<PhotoService> _logger;

        public PhotoService(IPhotoRepository photoRepository, ILogger<PhotoService> logger)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CreateNewPhoto(PhotoModel photoModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePhoto(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PhotoModel>> GetAll()
        {
            try
            {
                return await this._photoRepository.GetAll();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Can't get all photos, Error Message = {ex.Message}");
                throw;
            }
        }

        public async Task<PhotoModel> GetPhotoById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
