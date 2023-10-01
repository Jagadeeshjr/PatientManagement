using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Model;

namespace PatientManagement.Data
{
    public class PatientDBContext : IdentityDbContext<ApplicationUser>
    {
        public PatientDBContext(DbContextOptions<PatientDBContext> options)
            :base(options) 
        {

        }
        public DbSet<Patient> Patients { get; set; }
    }
}
