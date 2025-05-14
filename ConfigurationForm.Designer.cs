namespace CloudflareDDNService
{
    partial class ConfigurationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtApiKey = new MaterialSkin.Controls.MaterialTextBox();
            this.txtEmail = new MaterialSkin.Controls.MaterialTextBox();
            this.txtDomain = new MaterialSkin.Controls.MaterialTextBox();
            this.numUpdateInterval = new MaterialSkin.Controls.MaterialTextBox();
            this.lblCurrentIp = new MaterialSkin.Controls.MaterialLabel();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.btnUpdateNow = new MaterialSkin.Controls.MaterialButton();
            this.btnRefreshIp = new MaterialSkin.Controls.MaterialButton();
            this.btnViewLogs = new MaterialSkin.Controls.MaterialButton();
            
            // txtApiKey
            this.txtApiKey.Hint = "API Key";
            this.txtApiKey.Location = new System.Drawing.Point(20, 100);
            this.txtApiKey.Size = new System.Drawing.Size(350, 50);
            
            // txtEmail
            this.txtEmail.Hint = "Email";
            this.txtEmail.Location = new System.Drawing.Point(20, 160);
            this.txtEmail.Size = new System.Drawing.Size(350, 50);
            
            // txtDomain
            this.txtDomain.Hint = "Domain";
            this.txtDomain.Location = new System.Drawing.Point(20, 220);
            this.txtDomain.Size = new System.Drawing.Size(350, 50);
            
            // numUpdateInterval
            this.numUpdateInterval.Hint = "Update Interval (minutes)";
            this.numUpdateInterval.Location = new System.Drawing.Point(20, 280);
            this.numUpdateInterval.Size = new System.Drawing.Size(350, 50);
            
            // lblCurrentIp
            this.lblCurrentIp.Text = "Current IP: Loading...";
            this.lblCurrentIp.Location = new System.Drawing.Point(20, 340);
            this.lblCurrentIp.Size = new System.Drawing.Size(350, 30);
            
            // btnSave
            this.btnSave.Text = "Save Configuration";
            this.btnSave.Location = new System.Drawing.Point(20, 380);
            this.btnSave.Size = new System.Drawing.Size(170, 40);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnUpdateNow
            this.btnUpdateNow.Text = "Update DNS Now";
            this.btnUpdateNow.Location = new System.Drawing.Point(200, 380);
            this.btnUpdateNow.Size = new System.Drawing.Size(170, 40);
            this.btnUpdateNow.Click += new System.EventHandler(this.btnUpdateNow_Click);
            
            // btnRefreshIp
            this.btnRefreshIp.Text = "Refresh IP";
            this.btnRefreshIp.Location = new System.Drawing.Point(20, 430);
            this.btnRefreshIp.Size = new System.Drawing.Size(170, 40);
            this.btnRefreshIp.Click += new System.EventHandler(this.btnRefreshIp_Click);
            
            // btnViewLogs
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.Location = new System.Drawing.Point(200, 430);
            this.btnViewLogs.Size = new System.Drawing.Size(170, 40);
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            
            // ConfigurationForm
            this.ClientSize = new System.Drawing.Size(390, 490);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.numUpdateInterval);
            this.Controls.Add(this.lblCurrentIp);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdateNow);
            this.Controls.Add(this.btnRefreshIp);
            this.Controls.Add(this.btnViewLogs);
            this.Name = "ConfigurationForm";
            this.Text = "Cloudflare DDNS Configuration";
            this.ResumeLayout(false);
        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox txtApiKey;
        private MaterialSkin.Controls.MaterialTextBox txtEmail;
        private MaterialSkin.Controls.MaterialTextBox txtDomain;
        private MaterialSkin.Controls.MaterialTextBox numUpdateInterval;
        private MaterialSkin.Controls.MaterialLabel lblCurrentIp;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private MaterialSkin.Controls.MaterialButton btnUpdateNow;
        private MaterialSkin.Controls.MaterialButton btnRefreshIp;
        private MaterialSkin.Controls.MaterialButton btnViewLogs;
    }
}
