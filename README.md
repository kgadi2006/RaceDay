# RaceDay System

## Project Description
RaceDay is a web-based event management system developed to help organisers manage races and participants register for events. The system supports event management, participant enrolments, and race results.

## User Roles

### Organizer
- Create, update, and delete events.
- Manage race events and results.
- View event information.

### Participant
- View available events.
- Register for events and select a category.
- View personal enrolments and results.

## Technologies Used
- C#
- ASP.NET Core Web API
- .NET 8
- SQL Server
- JWT authentication
- xUnit for unit testing
- GitHub Actions for automated builds and tests

## How to Run the Project
1. Clone or download this repository.
2. Open `RaceDay.sln` in Visual Studio.
3. Check the database connection string in `appsettings.json` and update it for your SQL Server setup.
4. Ensure the required database and tables exist.
5. Restore NuGet packages.
6. Build the solution.
7. Run the `RaceDay` API project.
8. Open Swagger to test the API endpoints.

## How to Run the Tests
1. Open `RaceDay.sln` in Visual Studio.
2. Open **Test Explorer**.
3. Select **Run All Tests**.
4. Check the test results.

You can also run the tests from a terminal in the repository folder:

```bash
dotnet test RaceDay.sln --configuration Release
```

## Automated Testing
GitHub Actions automatically builds the solution and runs the unit tests when code is pushed or a pull request is created.

The latest workflow run completed successfully.

## Project Video
https://youtu.be/vp7EyI5AK4w?si=zWSi8wevtbe-NvPq
