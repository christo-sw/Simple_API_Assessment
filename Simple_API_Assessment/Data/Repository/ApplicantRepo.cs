using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Simple_API_Assessment.Interfaces;
using Simple_API_Assessment.Models;

namespace Simple_API_Assessment.Data.Repository
{
  public class ApplicantRepo : IApplicantRepository
  {
    private readonly DataContext _context;

    public ApplicantRepo(IOptions<ConnectionSettings> connectionSettings)
    {
      var options = new DbContextOptionsBuilder<DataContext>();
      options.UseNpgsql(connectionSettings.Value.DefaultConnection);

      _context = new DataContext(options.Options);
    }

    /// <summary>
    /// Gets all applicants with their skills
    /// </summary>
    /// <returns>A collection of applicants</returns>
    public ICollection<Applicant> GetApplicants()
    {
      return _context.Applicants.Include(applicant => applicant.Skills).ToList();
    }

    /// <summary>
    /// Gets an applicant by their ID
    /// </summary>
    /// <param name="id">The ID of the applicant to find</param>
    /// <returns>The applicant if it exists, otherwise null</returns>
    public Applicant? GetApplicant(int id)
    {
      return _context.Applicants.Where(applicant => applicant.Id == id).Include(applicant => applicant.Skills).FirstOrDefault();
    }

    /// <summary>
    /// Creates an applicant
    /// </summary>
    /// <param name="applicant">The applicant to be created, without any IDs</param>
    /// <returns>The created applicant</returns>
    public Applicant AddApplicant(ApplicantWithoutId applicant)
    {
      // Convert to regular skills
      List<Skill> skills = new();
      foreach (var skill in applicant.Skills)
      {
        skills.Add(new Skill() { Name = skill.Name });
      }

      // Convert to regular applicant
      var applicantToCreate = new Applicant
      {
        Name = applicant.Name,
        Skills = skills
      };

      var returnApplicant = _context.Applicants.Add(applicantToCreate);
      _context.SaveChanges();

      return returnApplicant.Entity;
    }

    /// <summary>
    /// Updates an applicant
    /// </summary>
    /// <param name="id">The ID of the applicant to update</param>
    /// <param name="applicant">The applicant that contains the changes to be applied, without any IDs</param>
    /// <returns>The updated applicant if successful or null if the applicant does not exist</returns>
    public Applicant? UpdateApplicant(int id, ApplicantWithoutId applicant)
    {
      var applicantToUpdate = _context.Applicants.Where(applicant => applicant.Id == id).Include(applicant => applicant.Skills).FirstOrDefault();
      if (applicantToUpdate == null)
      {
        return null;
      }

      // Convert to regular skills
      List<Skill> newSkills = new();
      foreach (var skill in applicant.Skills)
      {
        newSkills.Add(new Skill() { Name = skill.Name });
      }

      // Delete old applicant skills (more performant than trying to merge skill lists, particularly for large skill lists)
      _context.RemoveRange(applicantToUpdate.Skills);

      applicantToUpdate.Name = applicant.Name;
      applicantToUpdate.Skills = newSkills;

      _context.SaveChanges();
      return applicantToUpdate;
    }

    /// <summary>
    /// Deletes an applicant if it exists
    /// </summary>
    /// <param name="id">The ID of the applicant to be deleted</param>
    /// <returns>True if the applicant was successfully deleted, false if the applicant did not exist</returns>
    public bool RemoveApplicant(int id)
    {
      var applicant = _context.Applicants.Where(applicant => applicant.Id == id).Include(applicant => applicant.Skills).FirstOrDefault();

      if (applicant == null)
      {
        return false;
      }

      _context.RemoveRange(applicant.Skills);
      _context.Remove(applicant);
      _context.SaveChanges(true);
      return true;
    }
  }
}
