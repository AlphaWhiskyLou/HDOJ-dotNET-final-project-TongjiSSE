using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Question
    {
        public long QuestionId { get; set; }
        public string? ArgsType { get; set; }
        public string? Description { get; set; }
        public string? Difficulty { get; set; }
        public string? Labels { get; set; }
        public string? Name { get; set; }
        public long? Pass { get; set; }
        public string? ReturnType { get; set; }
        public long? Submission { get; set; }
    }
}
