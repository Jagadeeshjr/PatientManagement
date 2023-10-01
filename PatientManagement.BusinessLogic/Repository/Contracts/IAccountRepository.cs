using Microsoft.AspNetCore.Identity;
using PatientManagement.Models.Model.Authentication;

namespace PatientManagement.BusinessLogic.Repository.Contracts
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);

        Task<string> LoginAsync(SignInModel signInModel);
    }
}