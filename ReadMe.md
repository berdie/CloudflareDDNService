# =============================================
# <img src="https://icon.icepanel.io/Technology/svg/Cloudflare.svg" width="50" /> CLOUDFLARE DDNS SERVICE - INSTRUCTIONS
# =============================================

This application allows you to automatically update Cloudflare DNS records
with the public IP address of your device.

### SERVICE INSTALLATION AND MANAGEMENT
-------------------------------------

To easily manage all operations, use the file:
   ServiceControl.bat  (Run as administrator)

This script provides an interface to perform all necessary
operations on the service.

### MANUAL INSTALLATION
---------------------

1. Right-click on "InstallService.bat"
2. Select "Run as administrator"
3. Follow the on-screen instructions

The service will be installed and configured for automatic startup.

### CONFIGURATION
-------------

1. Start the CloudflareDDNService.exe application
2. Configure:
   - Cloudflare API Key
   - Cloudflare account Email address
   - Domain name (e.g., example.com)
   - Update interval (in minutes)

Configuration data is saved in:
%APPDATA%\CloudflareDDNService\config.json

### LOG FILE
-----------

Service logs are available in:
%APPDATA%\CloudflareDDNService\Logs\service.log

You can view them through the management menu or directly
by opening the file.

### UNINSTALLATION
---------------

1. Right-click on "UninstallService.bat"
2. Select "Run as administrator"
3. Follow the on-screen instructions

To completely remove the application, also delete:
%APPDATA%\CloudflareDDNService\

### TROUBLESHOOTING
-------------------

If the service does not start:
1. Verify that the configuration has been completed correctly
2. Check the log files for any errors
3. Use QueryService.bat to check the status of the service

If the icon in the taskbar disappears:
1. Restart the CloudflareDDNService.exe application

### =================================================
