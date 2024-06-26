using Microsoft.AspNetCore.Mvc;
using Simple_API_Assessment.Interfaces;
using Simple_API_Assessment.Models;

namespace Simple_API_Assessment.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ApplicantsController : Controller
  {
    private readonly IApplicantRepository _applicantRepository;

    public ApplicantsController(IApplicantRepository applicantRepository)
    {
      _applicantRepository = applicantRepository;
    }

    /// <summary>
    /// Gets all the applicants with their skills
    /// </summary>
    /// <returns>A list of applicants, each with a list of their respective skills</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Applicant>))]
    public IActionResult GetApplicants()
    {
      return Ok(_applicantRepository.GetApplicants());
    }

    /// <summary>
    /// Gets an applicant with their skills
    /// </summary>
    /// <param name="applicantId">The ID of the applicant to retrieve</param>
    /// <returns>The specified applicant with their skills if they exist, else a 404 with an explanation</returns>
    [HttpGet("{applicantId}")]
    [ProducesResponseType(200, Type = typeof(ICollection<Applicant>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404, Type = typeof(string))]
    public IActionResult GetApplicant(int applicantId)
    {
      var applicant = _applicantRepository.GetApplicant(applicantId);
      if (applicant == null)
      {
        return NotFound("Could not find applicant");
      }

      return Ok(applicant);
    }

    /// <summary>
    /// Creates an applicant with the specified details
    /// </summary>
    /// <param name="applicant">The applicant to create without any IDs</param>
    /// <returns>The created applicant with their skills</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(string))]
    [ProducesResponseType(400)]
    public IActionResult AddApplicant([FromBody] ApplicantWithoutId applicant)
    {
      var createdApplicant = _applicantRepository.AddApplicant(applicant);

      return Created("Successfully created applicant", createdApplicant);
    }

    /// <summary>
    /// Updates an applicant with the provided information
    /// </summary>
    /// <param name="applicantId">The ID of the applicant to update</param>
    /// <param name="applicant">The applicant with the new information added, without any IDs</param>
    /// <returns>The updated applicant if they exist, else a 404 with an explanation</returns>
    [HttpPatch("{applicantId}")]
    [ProducesResponseType(200, Type = typeof(Applicant))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404, Type = typeof(string))]
    public IActionResult UpdateApplicant(int applicantId, [FromBody] ApplicantWithoutId applicant)
    {
      var updatedApplicant = _applicantRepository.UpdateApplicant(applicantId, applicant);
      if (updatedApplicant == null)
      {
        return NotFound("Could not find applicant");
      }

      return Ok(updatedApplicant);
    }

    /// <summary>
    /// Deletes the specified applicant
    /// </summary>
    /// <param name="applicantId">The ID of the applicant to be deleted</param>
    /// <returns>A 200 if the applicant was deleted successfully, else a 404 with an explanation</returns>
    [HttpDelete("{applicantId}")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404, Type = typeof(string))]
    public IActionResult RemoveApplicant(int applicantId)
    {
      var success = _applicantRepository.RemoveApplicant(applicantId);
      if (!success)
      {
        return NotFound("Could not find applicant");
      }

      return Ok("Successfully deleted applicant");
    }
  }
}
