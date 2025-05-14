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
            this.SuspendLayout();
            // 
            // txtApiKey
            // 
            this.txtApiKey.AnimateReadOnly = false;
            this.txtApiKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtApiKey.Depth = 0;
            this.txtApiKey.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtApiKey.Hint = "API Key";
            this.txtApiKey.LeadingIcon = null;
            this.txtApiKey.Location = new System.Drawing.Point(20, 100);
            this.txtApiKey.MaxLength = 50;
            this.txtApiKey.MouseState = MaterialSkin.MouseState.OUT;
            this.txtApiKey.Multiline = false;
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(350, 50);
            this.txtApiKey.TabIndex = 0;
            this.txtApiKey.Text = "";
            this.txtApiKey.TrailingIcon = null;
            // 
            // txtEmail
            // 
            this.txtEmail.AnimateReadOnly = false;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Depth = 0;
            this.txtEmail.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEmail.Hint = "Email";
            this.txtEmail.LeadingIcon = null;
            this.txtEmail.Location = new System.Drawing.Point(20, 160);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.MouseState = MaterialSkin.MouseState.OUT;
            this.txtEmail.Multiline = false;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(350, 50);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.Text = "";
            this.txtEmail.TrailingIcon = null;
            // 
            // txtDomain
            // 
            this.txtDomain.AnimateReadOnly = false;
            this.txtDomain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDomain.Depth = 0;
            this.txtDomain.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDomain.Hint = "Domain";
            this.txtDomain.LeadingIcon = null;
            this.txtDomain.Location = new System.Drawing.Point(20, 220);
            this.txtDomain.MaxLength = 50;
            this.txtDomain.MouseState = MaterialSkin.MouseState.OUT;
            this.txtDomain.Multiline = false;
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(350, 50);
            this.txtDomain.TabIndex = 2;
            this.txtDomain.Text = "";
            this.txtDomain.TrailingIcon = null;
            // 
            // numUpdateInterval
            // 
            this.numUpdateInterval.AnimateReadOnly = false;
            this.numUpdateInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numUpdateInterval.Depth = 0;
            this.numUpdateInterval.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numUpdateInterval.Hint = "Update Interval (minutes)";
            this.numUpdateInterval.LeadingIcon = null;
            this.numUpdateInterval.Location = new System.Drawing.Point(20, 280);
            this.numUpdateInterval.MaxLength = 50;
            this.numUpdateInterval.MouseState = MaterialSkin.MouseState.OUT;
            this.numUpdateInterval.Multiline = false;
            this.numUpdateInterval.Name = "numUpdateInterval";
            this.numUpdateInterval.Size = new System.Drawing.Size(350, 50);
            this.numUpdateInterval.TabIndex = 3;
            this.numUpdateInterval.Text = "";
            this.numUpdateInterval.TrailingIcon = null;
            // 
            // lblCurrentIp
            // 
            this.lblCurrentIp.Depth = 0;
            this.lblCurrentIp.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurrentIp.Location = new System.Drawing.Point(20, 340);
            this.lblCurrentIp.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblCurrentIp.Name = "lblCurrentIp";
            this.lblCurrentIp.Size = new System.Drawing.Size(350, 30);
            this.lblCurrentIp.TabIndex = 4;
            this.lblCurrentIp.Text = "Current IP: Loading...";
            // 
            // btnSave
            // 
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnSave.Depth = 0;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(20, 380);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnSave.Size = new System.Drawing.Size(177, 36);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save Configuration";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSave.UseAccentColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdateNow
            // 
            this.btnUpdateNow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUpdateNow.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnUpdateNow.Depth = 0;
            this.btnUpdateNow.HighEmphasis = true;
            this.btnUpdateNow.Icon = null;
            this.btnUpdateNow.Location = new System.Drawing.Point(200, 380);
            this.btnUpdateNow.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnUpdateNow.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnUpdateNow.Name = "btnUpdateNow";
            this.btnUpdateNow.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnUpdateNow.Size = new System.Drawing.Size(147, 36);
            this.btnUpdateNow.TabIndex = 6;
            this.btnUpdateNow.Text = "Update DNS Now";
            this.btnUpdateNow.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnUpdateNow.UseAccentColor = false;
            this.btnUpdateNow.Click += new System.EventHandler(this.btnUpdateNow_Click);
            // 
            // btnRefreshIp
            // 
            this.btnRefreshIp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefreshIp.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnRefreshIp.Depth = 0;
            this.btnRefreshIp.HighEmphasis = true;
            this.btnRefreshIp.Icon = null;
            this.btnRefreshIp.Location = new System.Drawing.Point(20, 430);
            this.btnRefreshIp.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRefreshIp.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRefreshIp.Name = "btnRefreshIp";
            this.btnRefreshIp.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnRefreshIp.Size = new System.Drawing.Size(101, 36);
            this.btnRefreshIp.TabIndex = 7;
            this.btnRefreshIp.Text = "Refresh IP";
            this.btnRefreshIp.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnRefreshIp.UseAccentColor = false;
            this.btnRefreshIp.Click += new System.EventHandler(this.btnRefreshIp_Click);
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewLogs.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnViewLogs.Depth = 0;
            this.btnViewLogs.HighEmphasis = true;
            this.btnViewLogs.Icon = null;
            this.btnViewLogs.Location = new System.Drawing.Point(200, 430);
            this.btnViewLogs.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnViewLogs.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnViewLogs.Size = new System.Drawing.Size(97, 36);
            this.btnViewLogs.TabIndex = 8;
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnViewLogs.UseAccentColor = false;
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            // 
            // ConfigurationForm
            // 
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
            this.PerformLayout();

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
