namespace Simple_API_Assessment.Models
{
  public class ApplicantWithoutId
  {
    public string Name { get; set; }
    public ICollection<SkillWithoutId> Skills { get; set; }
  }
}
