setlocal
set dnuName=dnu.cmd
set dnxName=dnx.exe
set dnxVersion=dnx-clr-win-x64.1.0.0-rc1-update1
set dnxCorePath=%userprofile%\.dnx\runtimes\%dnxVersion%\bin\
set currentProject=Listening\src\Listening\
::set deploy=%dnxCorePath%%dnxName% --debug -p %currentProject%project.json web --server.urls http://0.0.0.0:8181

set deploy=%dnxCorePath%%dnxName% --debug -p %currentProject%project.json web --server.urls http://0.0.0.0:65531

%deploy%