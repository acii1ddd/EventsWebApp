Start-up instructions:

1. Install Docker desktop https://docs.docker.com/desktop/install/windows-install/

2. Clone the repository:
    git clone https://github.com/acii1ddd/EventsWebApp.git

4. Go to repository directory
   cd your-repository-name

5. At the root directory, restore required packages:
   dotnet restore

6. Next, build the solution:
    dotnet build

8. Go to docker directory:
   cd docker

9. Start up the application stack:
    docker compose up -d

10. Go to API:
   cd ../src/EventsApp.API

11. Launch App:
    dotnet run --launch-profile "https"

12. Go to https://localhost:7242/swagger/index.html
