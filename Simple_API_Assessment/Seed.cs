using Simple_API_Assessment.Data;
using Simple_API_Assessment.Models;

namespace Simple_API_Assessment
{
  public class Seed
  {
    private readonly DataContext _dataContext;

    public Seed(DataContext context)
    {
      _dataContext = context;
    }

    public void SeedDataContext()
    {
      if (!_dataContext.Applicants.Any() && !_dataContext.Skills.Any())
      {
        var skills = new List<Skill>()
        {
          new Skill()
          {
            Id = 1,
            Name = "Programming"
          },
          new Skill()
          {
            Id = 2,
            Name = "Problem solving"
          },
          new Skill()
          {
            Id = 3,
            Name = "Tenacity"
          }
        };

        var applicant = new Applicant()
        {
          Id = 1,
          Name = "Christo Swanepoel",
          Skills = skills
        };

        _dataContext.Applicants.Add(applicant);
        _dataContext.SaveChanges();
      }
    }
  }
}
