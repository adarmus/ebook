@echo off
echo Building Web
REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe /m "ebook.sln" /p:platform="Any CPU" /p:configuration="Release" /p:DeployOnBuild=true /p:PublishProfile=FileSystem /fileLogger1 /P:Framework40Dir=c:\windows\microsoft.net\framework\v4.0.30319

"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" /m "ebook.sln" /p:platform="Any CPU" /p:configuration="Release" /p:DeployOnBuild=true /fileLogger1 /P:Framework40Dir=c:\windows\microsoft.net\framework\v4.0.30319

PUSHD ebook\bin\Release
"C:\Program Files\7-Zip\7z.exe" a -tzip "..\..\..\ebook.zip" * -x!*.locked -x!*.pdb -x!*.xml -x!*.log -x!*.vshost.*
POPD

ECHO.
ECHO Copy to Dropbox ?
ECHO.

PAUSE

ECHO Copying...

COPY "ebook.zip" C:\Users\adam.blair\Dropbox

ECHO Copying...done

PAUSE
