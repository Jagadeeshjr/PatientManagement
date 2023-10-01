using Microsoft.AspNetCore.JsonPatch;
using PatientManagement.Model;

namespace PatientManagement.Repository
{
    public interface IPatientRepository
    {
        Task<PagedPatientResult> GetAllPatientsBySortingAsync(string term, string? sort, int page, int limit);

        Task<List<Patient>> GetPatientsAsync();

        Task<Patient> GetPatientByIdAsync(int id);

        Task<bool> PatientExistsAsync(int id);

        Task<int> AddPatientAsync(Patient patientModel);

        Task UpdatePatientAsync(Patient patientModel);

        Task UpdatePatientPatchAsync(int id, JsonPatchDocument patientModel);

        Task<bool> DeletePatientAsync(int id);
    }
}