using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Reference
    {
        public long ReferenceId { get; set; }
        public string? Introduction { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
    }
}
