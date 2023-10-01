using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.BusinessLogic.Repository.Contracts;
using PatientManagement.Model;

namespace PatientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatientsFiltering(string? term, string? sort, int page = 1, int limit = 10)
        {

            var sortRecords = await _patientRepository.GetAllPatientsBySortingAsync(term, sort, page, limit);

            if (sortRecords.Patients == null)
            {
                return NotFound();
            }

            return Ok(sortRecords.Patients);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientRepository.GetPatientsAsync();

            if (patients == null || patients.Count == 0)
            {
                return NotFound();
            }
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPatient([FromBody] Patient patientmodel) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var id = await _patientRepository.AddPatientAsync(patientmodel); ;
            return CreatedAtAction(nameof(GetPatientById), new { id, controller = "patient" }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient([FromRoute] int id , [FromBody]Patient patientModel) 
        {
            if (id <= 0 || id != patientModel.Id)
            {
                return BadRequest();
            }

            if (!await _patientRepository.PatientExistsAsync(id))
            {
                return NotFound();
            }

            await _patientRepository.UpdatePatientAsync(patientModel);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatientPatch([FromRoute] int id, [FromBody] JsonPatchDocument patientModel) 
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            await _patientRepository.UpdatePatientPatchAsync(id, patientModel);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient([FromRoute]int id)
        {
            var patient = await _patientRepository.DeletePatientAsync(id);

            if (patient != true)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
