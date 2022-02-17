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

        public PhotoService(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
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
            return await this._photoRepository.GetAll();
        }

        public async Task<PhotoModel> GetPhotoById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
