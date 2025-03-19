using Microsoft.AspNetCore.Identity;

namespace AssignmentPRN222.Models
{
    public class UserProfile :IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
