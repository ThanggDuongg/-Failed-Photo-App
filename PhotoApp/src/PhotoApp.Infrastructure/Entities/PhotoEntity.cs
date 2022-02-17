using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Infrastructure.Entities
{
    public enum PhotoMode {
        PRIVATE = 0,
        PUBLIC = 1,
    }

    public class PhotoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "PhotoId is invalid")]
        public Guid Id { get; init; }

        public PhotoMode Mode { get; set; }
        
        [Required(ErrorMessage = "Image is invalid")]
        public string Image { get; set; }

        [DataType(DataType.Text)]
        public string? Description { get; set; }

        public int Like { get; set; }

        public int Dislike { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        //public Guid UserId { get; set; }
    }
}
