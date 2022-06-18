using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Post

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Content { get; set; }
        public int CommentCount { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
