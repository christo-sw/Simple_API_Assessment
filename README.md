# Qubit Powerhouse Simple API Assessment 2024-06-26

**TODO**: Add explanation here

## Setup

**TODO**: Add steps for running DB and API

## Formatting

**TODO**: Explain formatting choices

## Notes

Installing the EntityFramework NuGet package was not enough to be able to extend `DbContext`, as it could not be found.
I had to manually install the package for the project using the command:

```console
dotnet add package EntityFramework --version 6.5.1
```
