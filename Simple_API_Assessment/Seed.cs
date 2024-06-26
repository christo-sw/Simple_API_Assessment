using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Simple_API_Assessment.Data;
using Simple_API_Assessment.Models;

namespace Simple_API_Assessment
{
  public class Seed
  {
    private readonly DataContext _context;

    public Seed(IOptions<ConnectionSettings> connectionSettings)
    {
      var options = new DbContextOptionsBuilder<DataContext>();
      options.UseNpgsql(connectionSettings.Value.DefaultConnection);

      _context = new DataContext(options.Options);
    }

    /// <summary>
    /// Seeds the database with the data that the assessment specified if the database is empty
    /// </summary>
    public void SeedDataContext()
    {
      if (!_context.Applicants.Any() && !_context.Skills.Any())
      {
        Console.WriteLine("The database is empty. Seeding...");
        var skills = new List<Skill>()
        {
          new Skill()
          {
            Name = "Programming"
          },
          new Skill()
          {
            Name = "Problem solving"
          },
          new Skill()
          {
            Name = "Tenacity"
          }
        };

        var applicant = new Applicant()
        {
          Name = "Christo Swanepoel",
          Skills = skills
        };

        _context.Applicants.Add(applicant);
        var count = _context.SaveChanges();
        if (count > 0)
        {
          Console.WriteLine($"Database seeded successfully with {count} entries");
        }
      }
    }
  }
}
