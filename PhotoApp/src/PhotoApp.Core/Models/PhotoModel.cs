using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Core.Models
{
    public enum PhotoMode
    {
        PRIVATE = 0,
        PUBLIC = 1,
    }

    public class PhotoModel
    {
        public PhotoMode Mode { get; set; }

        public string Image { get; set; }

        public string? Description { get; set; }

        public int Like { get; set; }

        public int Dislike { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
