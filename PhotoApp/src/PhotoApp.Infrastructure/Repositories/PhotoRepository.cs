using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhotoApp.Core.Interfaces.Repositories;
using PhotoApp.Core.Models;
using PhotoApp.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoAppDBContext _photoAppDBContext;
        private readonly IMapper _mapper;

        public PhotoRepository(PhotoAppDBContext photoAppDBContext, IMapper mapper)
        {
            _photoAppDBContext = photoAppDBContext ?? throw new ArgumentNullException(nameof(photoAppDBContext));
            _mapper = mapper;
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
            var dbPhoto = await this._photoAppDBContext.Photos.ToListAsync().ConfigureAwait(false);
            return this._mapper.Map<IEnumerable<PhotoModel>>(dbPhoto);
        }

        public async Task<PhotoModel> GetPhotoById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
