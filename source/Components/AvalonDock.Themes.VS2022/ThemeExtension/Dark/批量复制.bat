set /p prefix=enter prefix:
setlocal enabledelayedexpansion 
for /f %%f in ('dir /b Dark*.xaml') do (
    set h=%%f
    echo copy "%%f" "!prefix!!h:~4!"
)
endlocal
pause
