using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHub.Shared.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public int StepId { get; set; }
        public Step? Step { get; set; }

        [Required] 
        public string? Title { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        public string? Desc { get; set; }

        [NotMapped]
        public int Index { get; set; }
    }
}
