@echo off
setlocal

if not exist PublishDirectory.txt (
    echo Please create a file named PublishDirectory.txt and write the target directory path in it.
    pause
    exit /b
)

set /p targetDir=<PublishDirectory.txt

taskkill /f /im JeekNoteExplorer.exe >nul 2>nul
dotnet build --configuration Release &&^
xcopy /e /y bin "%targetDir%\" &&^
cd /d "%targetDir%" &&^
start "" JeekNoteExplorer.exe

endlocal
