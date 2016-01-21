promostudio
===========

This repository houses all PromoStudio source files and resources.

Required Setup
--------------

### Dev Environment ###
 - Install Visual Studio 2012 or higher
 - Install ASP.NET MVC4 via Visual Studio NuGet
 - Install [NodeJS](http://nodejs.org/)
   - Run `npm install` from `PromoStudio.Web` folder. This should install `less` and `requirejs`
   - Run `buildCss.bat` to create CSS files required for Web
   - Run `buildScripts.bat` to create optimized scripts required when Web has `UseOptimizedScripts` set to `True`
   - `build.bat` will build both CSS and scripts.

### Server Install Punchlist ###
 - Enable `Desktop Experience` on Win Server 2008/12
 - Install Chrome browser
 - Install Quicktime *(required on render server only)*
 - Install fonts used by templates *(required on render server only)*
 - Install Adobe After Effects CS6 *(required on render server only)*
 - Install [MySQL](http://dev.mysql.com/downloads/mirror.php?id=412168) *(full install on DB server only)*
   - Use sa account
 - Install FTP service using FTP Account credentials.
   - Setup folder permissions as appropriate
 - Install ASP.NET MVC4 *(required on web server only)*
 - Create service account for rendering service *(required on render server only)*
 - Install the PromoStudio Web Application in IIS. *(required on web server only)*
 - Install rendering service using service account, automated (delayed) startup, error recovery
 - Seed database via `Forward Engineer` command.

### RequiredAfterFX Output Formats ###

*When creating these output formats in AfterFX, you must be logged in as the `psservice` account as the output
formats are stored in the `ApplicationData` folder under that specific user account.*

**NTSC-H264**
 - _format info TBD_

**FullHD-H264**
 - _format info TBD_
