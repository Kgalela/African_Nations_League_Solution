# African_Nations_League_Solution

A web application for managing and simulating the African Nations League 2026.
The system is built with Blazor and .NET 8, MongoDB for data storage, and MudBlazor for the UI.

##Features

• Team registration with auto-generated squads
One registration slot is intentionally left open for a federation representative to submit their team. Once teams are loaded, an admin can log in to start the tournament and progress through each stage. Simulation can be done either in the admin dashboard or through the Tournament API in Swagger.

• Tournament bracket management
Supports Quarterfinals, Semifinals, and the Final. Admin users can simulate stages from the dashboard. Swagger also offers simulation actions for testing.

• Match simulation with results and commentary
The admin triggers match simulations, then can view the results and commentary from the match results screen.

• Admin dashboard
Central place to start or reset tournaments, simulate stages, review entries, and monitor progress.

• Email notifications (testing mode)
A match results summary is sent to the admin after the final simulation. Email currently sends only to the admin for development verification.

##Prerequisites

• .NET 8 SDK
• MongoDB (local or cloud instance such as Atlas)
• Visual Studio 2022 or later
• Blazor and MudBlazor libraries installed with the solution

##Architecture

This solution follows a Clean Architecture structure, specifically an onion-style separation of concerns.
If a UI component fails, the entire application does not crash because each layer is isolated.

The solution contains backend and frontend in separate folders. When running locally, set the API and WebAssembly projects to run together as Multiple Startup Projects:
src_Backend/
AfricanNationsLeague.Api → ASP.NET Core Web API
AfricanNationsLeague.Application → Services and business rules
AfricanNationsLeague.Infrastructure → Repositories and MongoDB context
AfricanNationsLeague.Domain → Entities and core models

srcUI_Frontend/
AfricanNationsLeague_Web → Blazor WebAssembly frontend
Getting Started

Clone the repository

git clone https://github.com/your-org/african-nations-league.git
cd african-nations-league

username for mangoDB :kgalela@dvtsoftware.com
password: 190794Khanyisa

Configure MongoDB
Ensure a MongoDB instance is running locally at:

mongodb://localhost:27017

If needed, update the connection string in
src_Backend/AfricanNationsLeague.Api/appsettings.json.

Run the backend

cd src_Backend/AfricanNationsLeague.Api
dotnet restore
dotnet run

The API will be available on the configured HTTPS port.
Swagger UI will be available for API testing.

Run the frontend
Start AfricanNationsLeague_Web through Visual Studio as a second startup project.
The Blazor app will open in your browser.

Access and Usage

• Navigate to the frontend URL to register teams and view the bracket
• Admin access via /admin/{email}
• Simulate stages and manage the tournament lifecycle
• Match results and commentary are visible after simulation

Environment Variables

Email notifications use Gmail settings configured in appsettings.json.
MongoDB connection settings are also stored there.

Development Notes

• Full dependency injection for services and repositories
• Tournament logic is implemented in the backend TournamentService
• UI is rendered with MudBlazor components

Troubleshooting

• Check that MongoDB is running and reachable
• Update API CORS rules if browser requests are blocked
• Verify Gmail credentials for email testing

License

MIT
Feel free to update to a different license if needed.

Thanks for checking out the African Nations League Simulator.
Have fun managing your own tournament!
