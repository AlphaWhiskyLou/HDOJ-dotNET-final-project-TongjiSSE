using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Code
    {
        public long CodeId { get; set; }
        public string? Content { get; set; }
        public string? Language { get; set; }
        public long? QuestionId { get; set; }
        public long? State { get; set; }
        public DateTime? SubmitTime { get; set; }
        public long? UserId { get; set; }
    }
}
