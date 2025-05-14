using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace CloudflareDDNService
{
    public partial class ConfigurationForm : MaterialForm
    {
        private readonly ConfigurationManager configManager;
        private readonly IpAddressProvider ipProvider;
        private readonly Logger logger;

        public ConfigurationForm()
        {
            InitializeComponent();
            
            // Setup MaterialSkin
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Orange800, 
                Primary.Orange900, 
                Primary.Orange500, 
                Accent.Orange200, 
                TextShade.WHITE);
            
            configManager = new ConfigurationManager();
            ipProvider = new IpAddressProvider();
            logger = new Logger();
            
            LoadConfiguration();
            UpdateCurrentIp();
        }

        private void LoadConfiguration()
        {
            var config = configManager.LoadConfiguration();
            
            txtApiKey.Text = config.ApiKey;
            txtEmail.Text = config.Email;
            txtDomain.Text = config.Domain;
            numUpdateInterval.Value = config.UpdateInterval;
        }

        private void UpdateCurrentIp()
        {
            try
            {
                string ip = ipProvider.GetPublicIpAddress();
                lblCurrentIp.Text = $"Current IP: {ip}";
            }
            catch (Exception ex)
            {
                lblCurrentIp.Text = "Failed to get current IP";
                logger.Log($"Error getting current IP: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var config = new Configuration
            {
                ApiKey = txtApiKey.Text,
                Email = txtEmail.Text,
                Domain = txtDomain.Text,
                UpdateInterval = (int)numUpdateInterval.Value
            };
            
            configManager.SaveConfiguration(config);
            MessageBox.Show("Configuration saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            // This would trigger the service to update DNS records
            // In a real implementation, you'd need to communicate with the service
            MessageBox.Show("Manual update triggered", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefreshIp_Click(object sender, EventArgs e)
        {
            UpdateCurrentIp();
        }
    }
}
