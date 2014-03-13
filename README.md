promostudio
===========

This repository houses all PromoStudio source files and resources.


Server Resources
----------------

#### Web/DB Server ###
**GoDaddy Account Name**: `PSTUDIODEV001`  
**Server IP Address**: `72.167.176.230`

#### Render Server ###
**GoDaddy Account Name**: `PSRENDER01`  
**Server IP Address**: `208.109.88.40`


Logins
------

#### Admin Account ####
**Username**: `psserver`  
**Password**: `pr0m0!g0ldMine`

#### Service Account ####
**Username**: `psservice`  
**Password**: `pr0m053rv!c3`

#### FTP Account ####
**Username**: `psftp`  
**Password**: `pr0m057ud!o`

#### MySQL Admin ####
**Username**: `sa`  
**Password**: `pr0m053rv!c3`

#### MySQL User ####
**Username**: `PromoStudioUser`  
**Password**: `pr0m057ud10`  
**Sample Connection String**: `Server=127.0.0.1;Database=promostudio;Uid=PromoStudioUser;Pwd=pr0m057ud10;`

#### Vimeo API ####
**VimeoApiClientId**: `d46e4b11bc1d52a70a3c86ce4014667c6f8a7816`  
**VimeoApiClientSecret**: `9386add8a2a2919c24c7a341ca673dbb1930d2c4`  
**VimeoApiAccessToken**: `2ab50b80be96906ad305a25414b36e2d`  
**VimeoApiAccessTokenSecret**: `f0feea423cb655950dc00b5c98651b122dedd877`


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
