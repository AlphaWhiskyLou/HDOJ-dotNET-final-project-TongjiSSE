using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Solution
    {
        public long SolutionId { get; set; }
        public string? Code { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Language { get; set; }
        public long? Likes { get; set; }
        public long? QuestionId { get; set; }
        public string? Title { get; set; }
        public long? UserId { get; set; }
    }
}
