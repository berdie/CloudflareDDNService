@echo off
setlocal enabledelayedexpansion

:MENU
cls
echo ========================================
echo     Gestione Servizio Cloudflare DDNS
echo ========================================
echo.
echo [1] Verifica stato del servizio
echo [2] Avvia il servizio
echo [3] Arresta il servizio
echo [4] Riavvia il servizio
echo [5] Visualizza log
echo [6] Apri configurazione manualmente
echo.
echo [7] Installa il servizio 
echo [8] Disinstalla il servizio
echo.
echo [0] Esci
echo.
echo ========================================

set /p choice=Seleziona un'opzione: 

if "%choice%"=="0" exit /b 0
if "%choice%"=="1" goto :STATUS
if "%choice%"=="2" goto :START
if "%choice%"=="3" goto :STOP
if "%choice%"=="4" goto :RESTART
if "%choice%"=="5" goto :VIEWLOG
if "%choice%"=="6" goto :CONFIG
if "%choice%"=="7" goto :INSTALL
if "%choice%"=="8" goto :UNINSTALL

echo Scelta non valida!
timeout /t 2 >nul
goto :MENU

:STATUS
cls
echo ========================================
echo        Stato del servizio
echo ========================================
echo.
sc.exe query CloudflareDDNService
echo.
echo ========================================
pause
goto :MENU

:START
cls
echo ========================================
echo        Avvio del servizio
echo ========================================
echo.
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERRORE: È necessario eseguire questo script come Amministratore.
    pause
    goto :MENU
)
sc.exe start CloudflareDDNService
echo.
echo ========================================
pause
goto :MENU

:STOP
cls
echo ========================================
echo        Arresto del servizio
echo ========================================
echo.
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERRORE: È necessario eseguire questo script come Amministratore.
    pause
    goto :MENU
)
sc.exe stop CloudflareDDNService
echo.
echo ========================================
pause
goto :MENU

:RESTART
cls
echo ========================================
echo        Riavvio del servizio
echo ========================================
echo.
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERRORE: È necessario eseguire questo script come Amministratore.
    pause
    goto :MENU
)
sc.exe stop CloudflareDDNService
timeout /t 5 /nobreak >nul
sc.exe start CloudflareDDNService
echo.
echo ========================================
pause
goto :MENU

:VIEWLOG
cls
echo ========================================
echo        Visualizza file di log
echo ========================================
echo.
if exist "%APPDATA%\CloudflareDDNService\Logs\service.log" (
    start notepad "%APPDATA%\CloudflareDDNService\Logs\service.log"
) else (
    echo ERRORE: File di log non trovato.
    echo Percorso atteso: %APPDATA%\CloudflareDDNService\Logs\service.log
    pause
)
goto :MENU

:CONFIG
cls
echo ========================================
echo        Apri configurazione
echo ========================================
echo.
if exist "%~dp0CloudflareDDNService.exe" (
    start "" "%~dp0CloudflareDDNService.exe"
) else (
    echo ERRORE: Eseguibile non trovato nella directory corrente.
    echo Verificare che lo script sia eseguito dalla directory dell'applicazione.
    pause
)
goto :MENU

:INSTALL
cls
echo ========================================
echo        Installazione del servizio
echo ========================================
echo.
if exist "InstallService.bat" (
    call InstallService.bat
) else (
    echo ERRORE: File InstallService.bat non trovato.
    pause
)
goto :MENU

:UNINSTALL
cls
echo ========================================
echo        Disinstallazione del servizio
echo ========================================
echo.
if exist "UninstallService.bat" (
    call UninstallService.bat
) else (
    echo ERRORE: File UninstallService.bat non trovato.
    pause
)
goto :MENU 