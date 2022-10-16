using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Submission
    {
        public string SubmissionId { get; set; } = null!;
        public long? UserId { get; set; }
        public long? QuestionId { get; set; }
        public string? Result { get; set; }
        public string? Language { get; set; }
        public DateTime? SubmissionTime { get; set; }
    }
}
