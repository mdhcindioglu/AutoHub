using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoHub.Shared.Entities
{
    public class Step
    {
        public int Id { get; set; }
        [Required] public string? Title { get; set; }

        [NotMapped] public bool Selected { get; set; }
        public List<Item>? Items { get; set; }
    }
}
