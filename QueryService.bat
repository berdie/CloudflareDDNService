@echo off
echo ========================================
echo Stato del servizio Cloudflare DDNS
echo ========================================
echo.

REM Verifica privilegi amministrativi
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo AVVISO: È consigliato eseguire questo script come Amministratore per visualizzare tutte le informazioni.
    echo Fare clic con il pulsante destro del mouse sul file .bat e selezionare "Esegui come amministratore".
    echo.
)

echo Controllo dell'esistenza del servizio...
sc.exe query CloudflareDDNService >nul 2>&1
if %errorLevel% neq 0 (
    echo ERRORE: Il servizio Cloudflare DDNS non risulta installato sul sistema.
    pause
    exit /b 1
)

echo.
echo === Informazioni di base sul servizio ===
sc.exe query CloudflareDDNService
echo.

echo === Configurazione del servizio ===
sc.exe qc CloudflareDDNService
echo.

echo === Visualizza percorso file di log ===
echo Il file di log è disponibile in:
echo %APPDATA%\CloudflareDDNService\Logs\service.log
echo.

echo === Visualizza ultima parte del file di log ===
if exist "%APPDATA%\CloudflareDDNService\Logs\service.log" (
    echo -- Ultime 10 righe del log --
    type "%APPDATA%\CloudflareDDNService\Logs\service.log" | findstr /n "^" | findstr /r ".[0-9][0-9]*:$" | findstr /b /e /l /r /c:"[0-9][0-9]*:" | more /e +%LINES%
) else (
    echo Il file di log non è stato ancora creato.
)

echo.
echo Per visualizzare il log completo: start notepad "%APPDATA%\CloudflareDDNService\Logs\service.log"
echo.
pause 