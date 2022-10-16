using System;
using System.Collections.Generic;

namespace DataModels.Models
{
    public partial class User
    {
        public long UserId { get; set; }
        public int? Age { get; set; }
        public byte[]? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Profile { get; set; }
        public string? SubmissionList { get; set; }
        public int? Type { get; set; }
        public string? University { get; set; }
        public string? Username { get; set; }
    }
}
