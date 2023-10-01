using Microsoft.AspNetCore.Identity;

namespace PatientManagement.Models.Model.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
