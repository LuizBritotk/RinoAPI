@echo off
setlocal

:: Set project name
set PROJECT_NAME=Rino

:: Create solution
dotnet new sln -n %PROJECT_NAME%

:: Create Domain project
dotnet new classlib -n %PROJECT_NAME%.Domain
cd %PROJECT_NAME%.Domain
mkdir Entities Interfaces Services
cd ..

:: Create Application project
dotnet new classlib -n %PROJECT_NAME%.Application
cd %PROJECT_NAME%.Application
mkdir Interfaces Services DTOs
cd ..

:: Create Infrastructure project
dotnet new classlib -n %PROJECT_NAME%.Infrastructure
cd %PROJECT_NAME%.Infrastructure
mkdir Repositories Data
cd ..

:: Create API project
dotnet new webapi -n %PROJECT_NAME%.API
cd %PROJECT_NAME%.API
mkdir Controllers Models
cd ..

:: Add projects to solution
dotnet sln %PROJECT_NAME%.sln add %PROJECT_NAME%.Domain\%PROJECT_NAME%.Domain.csproj
dotnet sln %PROJECT_NAME%.sln add %PROJECT_NAME%.Application\%PROJECT_NAME%.Application.csproj
dotnet sln %PROJECT_NAME%.sln add %PROJECT_NAME%.Infrastructure\%PROJECT_NAME%.Infrastructure.csproj
dotnet sln %PROJECT_NAME%.sln add %PROJECT_NAME%.API\%PROJECT_NAME%.API.csproj

:: Add project references
cd %PROJECT_NAME%.API
dotnet add reference ..\%PROJECT_NAME%.Application\%PROJECT_NAME%.Application.csproj
dotnet add reference ..\%PROJECT_NAME%.Infrastructure\%PROJECT_NAME%.Infrastructure.csproj
cd ..

cd %PROJECT_NAME%.Application
dotnet add reference ..\%PROJECT_NAME%.Domain\%PROJECT_NAME%.Domain.csproj
cd ..

cd %PROJECT_NAME%.Infrastructure
dotnet add reference ..\%PROJECT_NAME%.Domain\%PROJECT_NAME%.Domain.csproj
cd ..

echo Solution and projects created successfully.

endlocal
