using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SdaemonTestCrud.Models
{
    public class TaskItem
    {

        public int Id { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [DisplayName("Due Date")]
        [Required(ErrorMessage = "Due Date is required.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        
        public bool IsComplete { get; set; }

    }
}
