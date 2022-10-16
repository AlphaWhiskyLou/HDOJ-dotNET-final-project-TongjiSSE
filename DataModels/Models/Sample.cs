using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Sample
    {
        public long SampleId { get; set; }
        public string? Input { get; set; }
        public long? IsTest { get; set; }
        public string? Output { get; set; }
        public long? QuestionId { get; set; }
    }
}
