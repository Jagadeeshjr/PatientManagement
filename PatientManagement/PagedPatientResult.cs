using PatientManagement.Model;

namespace PatientManagement
{
    public class PagedPatientResult
    {
        public List<Patient> Patients { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
