# Sviluppo di un servizio Windows .Net Framework 4.8 per aggiornare i records di tipo A su Cloudflare.

#### Funzioni:
- Nome del servizio 'CloudflareDDNService' (sostituire a Service1.cs e impostare come oggetto di avvio ) 
 **Un interfaccia per:** 
- Inserire i dati per la connessione a Cloudflare 'ApiKey', 'Email', 'Dominio'.
- Consente di impostare il timer per l'aggiornamento, ma anche avere la possibilità
  di essere aggiornato manualmente. 
- Memorizzare i dati di configurazione.
- Visualizza l'attuale indirizzo IP dell Host.
- Visibile ed accessibile nella System Tray Icons con l'icona cloudflare.ico che si trova nella directory principale del progetto.

#### Altre funzioni per il servizio:
- Ottiene l'indirizzo IP da 'https://ipv4.icanhazip.com/' o 'https://api.ipify.org'
- Ottiene automaticamente lo zone_id e record_id usando il nome del dominio
- Aggiorna tutti i record di tipo A sul dominio
- Verifica degli indirizzi IP su Cloudflare e confronto con l'IP dell'Host
- Aggiornare gli indirizzi IP su Cloudflare se non corrispondono all'indirizzo IP dell' host
- Notifica se l'aggiornamento ha avuto successo o fallisce.
- Crea un file di log per il controllo degli aggiornamenti e il debug.
- Crea un sistema a rotazione che limita la dimensione dei file di log.
- Il progetto è in lingua Inglese.
- Per l'esecuzione sono richiesti i diritti di Amministratore
Utilizzo di MaterialSkin.2 per l'interfaccia e il tema 'OrangeDark'
MaterialSkin.2 e Newtonsoft.Json già installati