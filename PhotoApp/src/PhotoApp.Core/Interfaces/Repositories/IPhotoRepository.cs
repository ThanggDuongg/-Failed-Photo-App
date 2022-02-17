using PhotoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Core.Interfaces.Repositories
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<PhotoModel>> GetAll();

        Task<PhotoModel> GetPhotoById(int Id);

        Task<bool> CreateNewPhoto(PhotoModel photoModel);

        Task<bool> DeletePhoto(int Id);
    }
}
