using Microsoft.OpenApi.Models;
using Simple_API_Assessment;
using Simple_API_Assessment.Data.Repository;
using Simple_API_Assessment.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IApplicantRepository, ApplicantRepo>();
builder.Services.AddTransient<Seed>();

// Dependency inject the database connection string so that we do not have to inject the DataContext itself,
// allowing for more fine-grained database access control
builder.Services.Configure<ConnectionSettings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "Simple API Assesment",
    Description = "An assessment of a candidate's abilities to produce a simple .NET API",
    Contact = new OpenApiContact
    {
      Name = "Christo Swanepoel",
      Email = "xtoswanepoel@gmail.com",
    },
  });

  // Set the XML comments path for the Swagger JSON and UI.
  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Seed the database
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = scopedFactory.CreateScope())
{
  var service = scope.ServiceProvider.GetService<Seed>();
  service.SeedDataContext();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
