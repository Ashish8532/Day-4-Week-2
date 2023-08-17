﻿namespace BlogSystem.Models.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
