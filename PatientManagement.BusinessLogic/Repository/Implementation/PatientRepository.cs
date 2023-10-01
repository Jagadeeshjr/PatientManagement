using LazyCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PatientManagement.Caching;
using PatientManagement.Data;
using PatientManagement.Model;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using PatientManagement.BusinessLogic.Repository.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using PatientManagement.Models.Model;

namespace PatientManagement.BusinessLogic.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDBContext _context;
        private ICacheProvider _cacheProvider;


        public PatientRepository(PatientDBContext context, ICacheProvider cacheProvider)
        {
            _context = context;
            _cacheProvider = cacheProvider;
        }

        public async Task<PagedPatientResult> GetAllPatientsBySortingAsync(string term, string? sort, int page, int limit)
        {
            IQueryable<Patient> patients;
            if (string.IsNullOrWhiteSpace(term))
            {
                patients = _context.Patients;
            }
            else
            {
                term = term.Trim().ToLower();

                patients = _context
                    .Patients
                    .Where(x => x.FirstName.ToLower().Contains(term)
                    || x.LastName.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var sortFields = sort.Split(',');
                StringBuilder orderQueryBuilder = new StringBuilder();

                PropertyInfo[] propertyInfo = typeof(Patient).GetProperties();

                foreach (var field in sortFields)
                {
                    string sortOrder = "ascending";
                    var sortField = field.Trim();
                    if (sortField.StartsWith('-'))
                    {
                        sortField = sortField.TrimStart('-');
                        sortOrder = "descending";
                    }
                    var property = propertyInfo.FirstOrDefault(a =>
                                    a.Name.Equals(sortField,
                                    StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                    {
                        continue;
                    }
                    orderQueryBuilder.Append($"{property.Name.ToString()}" +
                                             $"{sortOrder}, ");
                }

                string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
                if (!string.IsNullOrWhiteSpace(orderQuery))
                {
                    patients = patients.OrderBy(orderQuery);
                }
                else
                {
                    patients = patients.OrderBy(x => x.Id);
                }
            }

            //applying pagination
            var totalCount = await _context.Patients.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var pagedPatients = await patients.Skip((page - 1) * limit).Take(limit).ToListAsync();

            var pagedPatientData = new PagedPatientResult
            {
                Patients = pagedPatients,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            return pagedPatientData;
        }

        public async Task<List<Patient>> GetPatientsAsync()
        {
            if (!_cacheProvider.TryGetValue(CacheKey.Patient, out List<Patient> patients))
            {
                patients = await _context.Patients.ToListAsync();

                var cacheEntryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1024
                };
                _cacheProvider.Set(CacheKey.Patient, patients, cacheEntryOption);
            }
            return patients;
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            if (!_cacheProvider.TryGetValue(CacheKey.Patient, out Patient patient))
            {
                patient = await _context.Patients.FindAsync(id);

                var cacheEntryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1024
                };
                _cacheProvider.Set(CacheKey.Patient, patient, cacheEntryOption);
            }
            return patient;
        }

        public async Task<bool> PatientExistsAsync(int id)
        {
            return await _context.Patients.AnyAsync(x => x.Id == id);
        }

        public async Task<int> AddPatientAsync(Patient patientModel)
        {
            patientModel.CreatedDate = DateTime.Now;
            _context.Patients.Add(patientModel);
            await _context.SaveChangesAsync();
            return patientModel.Id;
        }

        public async Task UpdatePatientAsync(Patient patientModel)
        {
            patientModel.UpdatedDate = DateTime.Now;
            _context.Patients.Update(patientModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientPatchAsync(int id, JsonPatchDocument patientModel)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient != null)
            {
                patient.UpdatedDate = DateTime.Now;
                patientModel.ApplyTo(patient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
