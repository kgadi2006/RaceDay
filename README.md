# RaceDay System

## Project Overview

RaceDay is a web-based race event management system developed using ASP.NET Core Web API and ASP.NET Core MVC.

The system allows organisers to manage race events, categories, participant enrolments and race results. Participants can browse events, register, enrol in events and view their results.

## Technologies Used

- C#
- ASP.NET Core Web API
- ASP.NET Core MVC
- .NET 8
- SQL Server
- Entity Framework Core / SQL
- HTML, CSS and Bootstrap
- GitHub Actions

## User Roles

### Organiser
- Register and log in.
- Create, view, update and delete events.
- Manage event categories.
- View participant enrolments.
- Capture and publish race results.

### Participant
- Register and log in.
- Browse available race events.
- View event details.
- Select categories and enrol in events.
- View and cancel enrolments.
- View and update profile information.
- View race results.

## Project Structure

The solution contains:

- **RaceDay API:** Handles data operations and business logic through REST API endpoints.
- **RaceDay MVC:** Provides the web-based user interface and communicates with the API.
- **RaceDayTests:** Contains unit tests for the system.

## Installation and Setup

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Configure the SQL Server connection string in the API project's `appsettings.json`.
4. Create or restore the required database.
5. Build the solution.
6. Run the API project.
7. Run the MVC project.
8. Register or log in to access the system.

## Testing

The project includes unit tests to check important system functionality, authentication and authorisation.

## Continuous Integration

GitHub Actions is used to automate the restoration, building and testing of the project.

## Demonstration Video

YouTube link: **[Add demonstration video link here]**

## Developer

Developed as part of the RaceDay System academic project.
