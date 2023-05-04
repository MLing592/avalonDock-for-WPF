@ECHO OFF
pushd "%~dp0"
::前两行命令均为执行命令前的准备工作，其中 @ECHO OFF 表示关闭命令行窗口输出，而 pushd "%~dp0" 主要是为了将当前执行路径设置为脚本所在的目录
ECHO.
ECHO.
ECHO.
ECHO This script deletes all temporary build files in their
ECHO corresponding BIN and OBJ Folder contained in the following projects
ECHO.
ECHO MLibTest
ECHO MLibTest_Components
ECHO.
ECHO MVVMTestApp
ECHO TestApp
ECHO WinFormsTestApp
ECHO.
ECHO Components\AvalonDock
ECHO Components\AvalonDock.Themes.Aero
ECHO Components\AvalonDock.Themes.Expression
ECHO Components\AvalonDock.Themes.VS2013
ECHO Components\AvalonDock.Themes.VS2010
ECHO Components\AvalonDock.Themes.Metro
ECHO.
ECHO AutomationTest\AvalonDockTest
ECHO CaliburnDockTestApp
ECHO AvalonDocPanelMemoryLeaks
ECHO.
::输出一些提示信息，其作用是告诉用户该脚本的用途和将会删除哪些项目中的临时构建文件
REM Ask the user if hes really sure to continue beyond this point XXXXXXXX
set /p choice=Are you sure to continue (Y/N)?
if not '%choice%'=='Y' Goto EndOfBatch
REM Script does not continue unless user types 'Y' in upper case letter
::实现了交互式操作，提示用户是否确定要继续执行删除操作。具体来说，它通过 set /p 命令向用户发起询问，并对用户输入进行判断。如果用户输入的是大写字母 Y，则脚本会跳转到下一行代码，否则跳转至 :EndOfBatch 标签处结束脚本运行。
ECHO.
ECHO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
ECHO.
ECHO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
ECHO.
ECHO Removing vs settings folder with *.sou file
ECHO.
RMDIR /S /Q .vs
::输出一些提示信息，并在本地文件系统中删除名为 .vs 的文件夹及其所有文件（使用 /S /Q 选项表示递归删除并关闭确认提示）
ECHO Deleting BIN and OBJ Folders in MLibTest
ECHO.
RMDIR /S /Q MLibTest\MLibTest\bin
RMDIR /S /Q MLibTest\MLibTest\obj

ECHO Deleting BIN and OBJ Folders in MLibTest_Components
ECHO.
RMDIR /S /Q MLibTest\MLibTest_Components\ServiceLocator\bin
RMDIR /S /Q MLibTest\MLibTest_Components\ServiceLocator\obj

RMDIR /S /Q MLibTest\MLibTest_Components\Settings\Settings\bin
RMDIR /S /Q MLibTest\MLibTest_Components\Settings\Settings\obj

RMDIR /S /Q MLibTest\MLibTest_Components\Settings\SettingsModel\bin
RMDIR /S /Q MLibTest\MLibTest_Components\Settings\SettingsModel\obj

ECHO Deleting BIN and OBJ Folders in MVVMTestApp
ECHO.
RMDIR /S /Q MVVMTestApp\bin
RMDIR /S /Q MVVMTestApp\obj

ECHO Deleting BIN and OBJ Folders in TestApp
ECHO.
RMDIR /S /Q TestApp\bin
RMDIR /S /Q TestApp\obj

ECHO Deleting BIN and OBJ Folders in WinFormsTestApp
ECHO.
RMDIR /S /Q WinFormsTestApp\bin
RMDIR /S /Q WinFormsTestApp\obj

ECHO Deleting BIN and OBJ Folders in AvalonDock.Themes.Aero
ECHO.
RMDIR /S /Q Components\AvalonDock.Themes.Aero\bin
RMDIR /S /Q Components\AvalonDock.Themes.Aero\obj

ECHO Deleting BIN and OBJ Folders in AvalonDock
ECHO.
RMDIR /S /Q Components\AvalonDock\bin
RMDIR /S /Q Components\AvalonDock\obj

ECHO Deleting BIN and OBJ Folders in AvalonDock.Themes.Expression

ECHO.
RMDIR /S /Q Components\AvalonDock.Themes.Expression\bin
RMDIR /S /Q Components\AvalonDock.Themes.Expression\obj

ECHO Deleting BIN and OBJ Folders in AvalonDock.Themes.VS2010
ECHO.
RMDIR /S /Q Components\AvalonDock.Themes.VS2010\bin
RMDIR /S /Q Components\AvalonDock.Themes.VS2010\obj

ECHO Deleting BIN and OBJ Folders in AvalonDock.Themes.VS2013
ECHO.
RMDIR /S /Q Components\AvalonDock.Themes.VS2013\bin
RMDIR /S /Q Components\AvalonDock.Themes.VS2013\obj

ECHO Deleting BIN and OBJ Folders in AvalonDock.Themes.Metro
ECHO.
RMDIR /S /Q Components\AvalonDock.Themes.Metro\bin
RMDIR /S /Q Components\AvalonDock.Themes.Metro\obj

ECHO Deleting BIN and OBJ Folders in AutomationTest\AvalonDockTest
ECHO.
RMDIR /S /Q AutomationTest\AvalonDockTest\bin
RMDIR /S /Q AutomationTest\AvalonDockTest\obj

ECHO Deleting BIN and OBJ Folders in CaliburnDockTestApp
ECHO.
RMDIR /S /Q CaliburnDockTestApp\bin
RMDIR /S /Q CaliburnDockTestApp\obj

ECHO Deleting BIN and OBJ Folders in AvalonDocPanelMemoryLeaks
ECHO.
RMDIR /S /Q AvalonDocPanelMemoryLeaks\bin
RMDIR /S /Q AvalonDocPanelMemoryLeaks\obj

PAUSE

:EndOfBatch
