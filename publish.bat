@echo off

set "AppName=sortxml"
set "ProjectFile=%AppName%.csproj"

cd "%~dp0"

dotnet publish "%ProjectFile%" -c Debug -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=false -o ./publish/win-x64
if %ERRORLEVEL% NEQ 0 pause & exit /B

if exist "%UserProfile%\Bin" (
	copy .\publish\win-x64\%AppName%.exe "%UserProfile%\Bin\"
	if %ERRORLEVEL% NEQ 0 pause
)
