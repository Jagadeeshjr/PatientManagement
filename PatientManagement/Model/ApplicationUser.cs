﻿using Microsoft.AspNetCore.Identity;

namespace PatientManagement.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
