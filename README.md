# Qubit Powerhouse Simple API Assessment 2024-06-26

The following is a simple assessment to demonstrate an applicant's understanding of
- C#
- .Net framework
- Entity Framework
- Databases
- Repository Design Pattern
- API development

## Setup

Clone the repo from GitHub using your preferred method, following the below steps after doing so.

### Database

I opted to dockerize the database and the API, since it is the most standard approach. This assumes
you have Docker Desktop installed (if on Windows), or atleast the Docker CLI. If you do not, and do not have
a database already set up to use, install Docker Desktop via [this link.](https://www.docker.com/products/docker-desktop/)

Now, if you wish to only run the database, run the following command in the Simple_API_Assessment directory (at the level where Program.cs is):

```console
docker compose -f docker-compose.DBOnly.yaml up -d
```

This creates the postgres database container and exposes it on port 5432. It should show in Docker Desktop in the following manner:

![image](https://github.com/christo-sw/Simple_API_Assessment/assets/103880515/73d79228-8fbe-42d5-a27d-3d09be45d60f)

Once you are done, you can remove the container with the following command:

```console
docker compose -f docker-compose.DBOnly.yaml down
```

If you would like to run both the API and the database at once via docker, use the following command:

```console
docker compose up -d
```

Once you are done, you can remove the containers with the following command:

```console
docker compose down
```

The database is seeded automatically if it is empty. The database container persists the database's data, so it is availabe
after a restart or even after deleting and recreating the database container.

NOTE: I did not use Visual Studio's container orchestration as a method of running both the API and the database. I tried doing so,
but it did not work off the bat. Unfortunately, I did not have time to go back and get it working.

### API

There are multiple ways in which to run the API. (like above with docker, one-shot out of VS, or manually using dotnet run)

To run the production build of the API in a docker container, follow the steps in the previous section.

If you wish to run the docker from Visual Studio, simply open the solution and click on the run button that says "Simple_API_Assessment":

![image](https://github.com/christo-sw/Simple_API_Assessment/assets/103880515/8f3b8fb8-0277-483d-9444-79cd80288a2c)

Lastly, if you wish to run the API from the console, open the project directory in the console (at the same level as Program.cs) and run the command:

```console
dotnet run
```

- To connect to the API, use `http://localhost:5049`
- To connect to SwaggerUI, use `http://localhost:7156`

NOTE: If running from Visual Studio, SwaggerUI will be available and open automatically. 
If running from the command line, SwaggerUI is available but has to be navigated to using the above link.
If running via docker, SwaggerUI will not be available, since the production build of the API does not host the UI.


## Formatting

I used Visual Studio's built-in formatter for this project. However, I did set my tabs to be spaces, since I believe this to be the better and more
consistent formatting choice. I also set my tab width to two spaces instead of four, simply due to personal preference.

## REST Principles

The assessment calls for applicant CRUD operations that each take an ID.
However, I do not believe it to be a good idea to allow for a creation (i.e. `POST`) endpoint that has an ID as a path parameter.
This is because I believe ID generation should be handled by the database, or the backend at the very least.
There might be considerable logic involved in deciding which ID should be used next. This presents the issue that the client does not
know all of the information regarding the applicant it just created, since it doesn't know its ID, nor the IDs of the applicant's skills.
The remedy is simple. We return the newly created applicant with its ID, as well as its skills and their IDs.
Now the client has the full state without needing to make a `GET` request.
Therefore, I opted to remove the ID path parameter from the applicant creation endpoint.

Furthermore, in keeping with REST convention, I opted to rename the `ApplicantController` to `ApplicantsController`,
since the latter produces the path `api/Applicants`. In this case, the plural form of the noun is preferrable, since it
clarifies that `GET api/Applicants` would return a list of applicants, not just a single applicant.
We then index into individual applicants using the `{applicantId}` path parameter, e.g. `GET api/Applicants/{applicantId}`.

Also, I opted to use the `PATCH` method for updating applicants, since it fits more closely with updating resources than `PUT` does.
`PUT` implies a completely new resource was created, which is not the case. Again, the newly updated applicant is returned upon the completion
of the request so that the client has the new state.

## Duplicate Types

While setting up the API I ran into the issue of how to handle the data that the client sends when making a request to create or
update an applicant. I do not want clients to specify IDs, as I explained above. I could simply ignore the IDs that the client sends
(which is what happens now), but Swagger's generated API documentation indicates that IDs should be present because it infers it from the
`Applicant` and `Skill` types.

My solution to this was to create two new types: `ApplicantWithoutId` and `SkillWithoutId`. They are the exact same as their normal counterparts,
except that they do not have ID fields, and `ApplicantWithoutId` has a collection of `SkillWithoutId` instead of `Skill`. I had to make a dedicated class
for these types so that I can use ASP.NET's `[FromBody]` attribute on the route's parameters. The `[FromBody]` attribute can only be used on a single parameter,
so the standard approach is to encapsulate your data in a new class, like I did, and then transform the data into the correct type before inserting
it into the database.

## Future Improvements

I did not have the chance to test the performance of this API. If I had more time, I would look at implementing `async` wherever necessary
to speed up the API by allowing for more concurrent requests.

I would also like to add a search endpoint that allows clients to search for different applicants or skills.

## Notes

I decided to use Entity Framework Core instead of Entity Framework 6.

I understand this goes against the assessment's requirements, but I had constant issues marrying .NET 6 and Entity Framework 6.
I had never done so before, so I tried searching on the internet for leads. There is surprisingly little information available on the internet regarding this combination.

Most resources describe using .NET 6 with Entity Framework Core. I think this is because Entity Framework 6 is significantly older than .NET 6, being released in 2013 vs 2021, respectively.
Very few resources would speak of using Entity Framework 6 alongside such a (relatively) new version of .NET.
Even Microsoft recommends using Entity Framework Core with .NET 6 instead of Entity Framework 6, as shown by [this link.](https://learn.microsoft.com/en-us/aspnet/core/data/entity-framework-6?view=aspnetcore-8.0)

Usually, in circumstances like these, I would turn to a senior for advice if the internet proves unfruitful. Unfortunately, that was not an option during this assessment.

That being said, I refused to give up! While not being exactly what the assessment wants, I substituted Entity Framework 6 with Entity Framework Core because I resolved to
deliver an excellent, working project, even if it is not completely to specification. I hope that you understand.

Thank you for the opportunity to complete this assessment. It has been immense fun, and I learned heaps more than I expected.
