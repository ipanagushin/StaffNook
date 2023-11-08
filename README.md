# StaffNook
Your Work, Your Way, Your Nook


# How add migration
`dotnet ef --startup-project ./src/StaffNook.Backend/StaffNook.Backend.csproj database update --context Context --output-dir Migrations --project ./src/StaffNook.Infrastructure/StaffNook.Infrastructure.csproj`