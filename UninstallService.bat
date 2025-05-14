@echo off
echo ========================================
echo Disinstallazione del servizio Cloudflare DDNS
echo ========================================
echo.

REM Verifica privilegi amministrativi
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERRORE: È necessario eseguire questo script come Amministratore.
    echo Fare clic con il pulsante destro del mouse sul file .bat e selezionare "Esegui come amministratore".
    pause
    exit /b 1
)

REM Controllo se il servizio esiste
sc.exe query CloudflareDDNService >nul 2>&1
if %errorLevel% neq 0 (
    echo Il servizio Cloudflare DDNS non risulta installato sul sistema.
    pause
    exit /b 0
)

echo Arresto del servizio in corso...
sc.exe stop CloudflareDDNService
timeout /t 5 /nobreak >nul

echo Verifica che il servizio sia effettivamente arrestato...
for /l %%i in (1,1,5) do (
    sc.exe query CloudflareDDNService | find "STATE" | find "STOPPED" >nul
    if !errorlevel! equ 0 (
        goto :SERVICE_STOPPED
    )
    timeout /t 2 /nobreak >nul
)

:SERVICE_STOPPED
echo Eliminazione del servizio in corso...
sc.exe delete CloudflareDDNService
if %errorLevel% neq 0 (
    echo ERRORE: Impossibile rimuovere il servizio. Verificare i permessi o se il servizio è ancora in esecuzione.
    pause
    exit /b 1
)

echo.
echo Disinstallazione completata con successo!
echo.
echo NOTA: I file di configurazione e i log non sono stati rimossi.
echo Se si desidera rimuoverli, eliminare manualmente la cartella:
echo %APPDATA%\CloudflareDDNService\
echo.
pause 