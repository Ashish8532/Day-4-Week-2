using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Models.Models
{
    public class Blog
    {
        [Key]
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
