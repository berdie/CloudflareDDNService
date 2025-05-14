=================================================
     CLOUDFLARE DDNS SERVICE - ISTRUZIONI
=================================================

Questa applicazione permette di aggiornare automaticamente i record DNS 
di Cloudflare con l'indirizzo IP pubblico del tuo dispositivo.

INSTALLAZIONE E GESTIONE DEL SERVIZIO
-------------------------------------

Per gestire facilmente tutte le operazioni, utilizzare il file:
   ServiceControl.bat  (Eseguire come amministratore)

Questo script fornisce un'interfaccia per eseguire tutte le operazioni
necessarie sul servizio.

INSTALLAZIONE MANUALE
---------------------

1. Fare clic con il tasto destro su "InstallService.bat"
2. Selezionare "Esegui come amministratore"
3. Seguire le istruzioni a schermo

Il servizio verrà installato e configurato per l'avvio automatico.

CONFIGURAZIONE
-------------

1. Avviare l'applicazione CloudflareDDNService.exe
2. Configurare:
   - API Key di Cloudflare
   - Indirizzo Email dell'account Cloudflare
   - Nome di dominio (es. esempio.com)
   - Intervallo di aggiornamento (in minuti)

I dati di configurazione sono salvati in:
%APPDATA%\CloudflareDDNService\config.json

FILE DI LOG
-----------

I log del servizio sono disponibili in:
%APPDATA%\CloudflareDDNService\Logs\service.log

È possibile visualizzarli tramite il menu di gestione o direttamente
aprendo il file.

DISINSTALLAZIONE
---------------

1. Fare clic con il tasto destro su "UninstallService.bat"
2. Selezionare "Esegui come amministratore"
3. Seguire le istruzioni a schermo

Per rimuovere completamente l'applicazione, eliminare anche:
%APPDATA%\CloudflareDDNService\

RISOLUZIONE PROBLEMI
-------------------

Se il servizio non si avvia:
1. Verificare che la configurazione sia stata completata correttamente
2. Controllare i file di log per eventuali errori
3. Usare QueryService.bat per verificare lo stato del servizio

Se l'icona nella barra delle applicazioni scompare:
1. Riavviare l'applicazione CloudflareDDNService.exe

================================================= 