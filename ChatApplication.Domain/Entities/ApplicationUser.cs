using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }
    }
}
