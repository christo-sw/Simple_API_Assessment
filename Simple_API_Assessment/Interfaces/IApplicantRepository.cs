using Simple_API_Assessment.Models;

namespace Simple_API_Assessment.Interfaces
{
  public interface IApplicantRepository
  {
    ICollection<Applicant> GetApplicants();
    Applicant? GetApplicant(int id);
    Applicant AddApplicant(ApplicantWithoutId applicant);
    Applicant? UpdateApplicant(int id, ApplicantWithoutId applicant);
    bool RemoveApplicant(int id);
  }
}
