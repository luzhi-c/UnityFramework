set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d bin ^
    -c cs-bin ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=%WORKSPACE%\Assets\GameMain\LubanData ^
    -x outputCodeDir=%WORKSPACE%\Assets\GameMain\Scripts\LubanCode 

pause