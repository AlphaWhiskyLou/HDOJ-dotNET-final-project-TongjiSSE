using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Comment
    {
        public long CommentId { get; set; }
        public DateTime? CommentTime { get; set; }
        public string? Content { get; set; }
        public long? Likes { get; set; }
        public long? SolutionId { get; set; }
        public long? UserId { get; set; }
    }
}
