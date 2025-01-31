using Microsoft.AspNetCore.Identity;

namespace E_Commers.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Adderss { get; set; }
    }
}
