using System.ComponentModel.DataAnnotations.Schema;

namespace Application.WebAPI.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
