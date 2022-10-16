using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class Solo
    {
        public long SoloId { get; set; }
        public long? UserId { get; set; }
        public int? Ranking { get; set; }
        public int? TotalCount { get; set; }
        public int? PassCount { get; set; }
    }
}
