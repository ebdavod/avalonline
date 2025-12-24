using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class ReviewMedia
    {
        public int Id { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required]
        public string MediaUrl { get; set; }

        public MediaType MediaType { get; set; }

        [ForeignKey("ReviewId")]
        public ProductReview Review { get; set; }
    }
}
