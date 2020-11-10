# Spatial Check
A small application for notifying the user if Spatial Sound is turned off on startup on Windows 10.

![Screenshot](screenshot.png)

# Installation
- Download the Release files and move them to a permanent location (e.g. `Program Files\Spatial Check`)
- Create a shortcut to `SpatialCheckTray.exe`
- Move the shortcut to `%AppData%\Microsoft\Windows\Start Menu\Programs\Startup` in order to check whether spatial audio is enabled at Windows startup

# Build
- Open the `.sln` in Visual Studio
- Choose the appropriate build target
- Build/Run

alternatively build using `MSBuild` with an appropriate target. 

<a rel="license" href="http://creativecommons.org/licenses/by-nc/4.0/"><img alt="Creative Commons License" style="border-width:0" src="https://i.creativecommons.org/l/by-nc/4.0/80x15.png" /></a>
