using Microsoft.AspNetCore.Identity;
using PatientManagement.Model;

namespace PatientManagement.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);

        Task<string> LoginAsync(SignInModel signInModel);
    }
}