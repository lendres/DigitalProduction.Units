@echo off

rem Get the api key.
set /p api-key=<"../../../Keys/Nuget/api-key.txt"
echo API KEY: %api-key%

rem Change to the package directory.
chdir /d ../bin/Release
echo %cd%

rem Find only the latest package version.
set "latest_file="
for /f %%a in ('call dir *.nupkg /b') do (
	set "latest_file=%%a"
)

rem Publish.
echo.
echo Processing: %latest_file%
dotnet nuget push %latest_file% --api-key %api-key% --source https://api.nuget.org/v3/index.json --skip-duplicate

echo.
pause