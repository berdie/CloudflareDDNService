@echo off
echo ========================================
echo Installazione del servizio Cloudflare DDNS
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

REM Ottieni il percorso dell'eseguibile corrente
set "SERVICE_PATH=%~dp0CloudflareDDNService.exe"

REM Verifica che l'eseguibile esista
if not exist "%SERVICE_PATH%" (
    echo ERRORE: Impossibile trovare l'eseguibile CloudflareDDNService.exe
    echo Assicurarsi di eseguire questo script dalla directory dell'applicazione.
    pause
    exit /b 1
)

echo Creazione del servizio Windows...
sc.exe create CloudflareDDNService binPath= "%SERVICE_PATH% --service" start= auto DisplayName= "Cloudflare DDNS Service"
if %errorLevel% neq 0 (
    echo ERRORE: Creazione del servizio fallita.
    pause
    exit /b 1
)

echo Impostazione della descrizione del servizio...
sc.exe description CloudflareDDNService "Updates Cloudflare DNS A records with the current public IP address"

echo Avvio del servizio...
sc.exe start CloudflareDDNService
if %errorLevel% neq 0 (
    echo AVVISO: Avvio del servizio fallito. Potrebbe essere necessario avviarlo manualmente.
)

echo.
echo Installazione completata con successo!
echo Il servizio verrà avviato automaticamente ad ogni riavvio del sistema.
echo Per configurare il servizio, eseguire l'applicazione CloudflareDDNService.exe.
echo.
pause 