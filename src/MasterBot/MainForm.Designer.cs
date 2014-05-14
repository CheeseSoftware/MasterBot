namespace MasterBot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMainTabs = new System.Windows.Forms.TabControl();
            this.tabPageLoginConnect = new System.Windows.Forms.TabPage();
            this.groupBoxCode = new System.Windows.Forms.GroupBox();
            this.buttonSendCode = new System.Windows.Forms.Button();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.groupBoxConnect = new System.Windows.Forms.GroupBox();
            this.comboBoxRoomId = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelRoomId = new System.Windows.Forms.Label();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.comboBoxEmail = new System.Windows.Forms.ComboBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.tabPageMinimap = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelMinimap = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBoxMinimap = new System.Windows.Forms.PictureBox();
            this.tabPageSubBots = new System.Windows.Forms.TabPage();
            this.tabControlSubBots = new System.Windows.Forms.TabControl();
            this.RtbConsoleInput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RtbConsole = new Gui.RickTextConsole();
            this.tabPageEnableDisableSubBots = new System.Windows.Forms.TabPage();
            this.groupBoxSubBots = new System.Windows.Forms.GroupBox();
            this.checkedListBoxSubBots = new System.Windows.Forms.CheckedListBox();
            this.tabControlMainTabs.SuspendLayout();
            this.tabPageLoginConnect.SuspendLayout();
            this.groupBoxCode.SuspendLayout();
            this.groupBoxConnect.SuspendLayout();
            this.groupBoxLogin.SuspendLayout();
            this.tabPageMinimap.SuspendLayout();
            this.flowLayoutPanelMinimap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMinimap)).BeginInit();
            this.tabPageSubBots.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPageEnableDisableSubBots.SuspendLayout();
            this.groupBoxSubBots.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMainTabs
            // 
            this.tabControlMainTabs.Controls.Add(this.tabPageLoginConnect);
            this.tabControlMainTabs.Controls.Add(this.tabPageMinimap);
            this.tabControlMainTabs.Controls.Add(this.tabPageEnableDisableSubBots);
            this.tabControlMainTabs.Controls.Add(this.tabPageSubBots);
            this.tabControlMainTabs.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tabControlMainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMainTabs.Location = new System.Drawing.Point(3, 3);
            this.tabControlMainTabs.Name = "tabControlMainTabs";
            this.tabControlMainTabs.SelectedIndex = 0;
            this.tabControlMainTabs.Size = new System.Drawing.Size(484, 456);
            this.tabControlMainTabs.TabIndex = 0;
            // 
            // tabPageLoginConnect
            // 
            this.tabPageLoginConnect.Controls.Add(this.groupBoxCode);
            this.tabPageLoginConnect.Controls.Add(this.groupBoxConnect);
            this.tabPageLoginConnect.Controls.Add(this.groupBoxLogin);
            this.tabPageLoginConnect.Location = new System.Drawing.Point(4, 22);
            this.tabPageLoginConnect.Name = "tabPageLoginConnect";
            this.tabPageLoginConnect.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLoginConnect.Size = new System.Drawing.Size(476, 430);
            this.tabPageLoginConnect.TabIndex = 0;
            this.tabPageLoginConnect.Text = "LoginConnect";
            this.tabPageLoginConnect.UseVisualStyleBackColor = true;
            // 
            // groupBoxCode
            // 
            this.groupBoxCode.Controls.Add(this.buttonSendCode);
            this.groupBoxCode.Controls.Add(this.textBoxCode);
            this.groupBoxCode.Controls.Add(this.labelCode);
            this.groupBoxCode.Location = new System.Drawing.Point(8, 190);
            this.groupBoxCode.Name = "groupBoxCode";
            this.groupBoxCode.Size = new System.Drawing.Size(243, 72);
            this.groupBoxCode.TabIndex = 7;
            this.groupBoxCode.TabStop = false;
            this.groupBoxCode.Text = "Code";
            // 
            // buttonSendCode
            // 
            this.buttonSendCode.Location = new System.Drawing.Point(162, 11);
            this.buttonSendCode.Name = "buttonSendCode";
            this.buttonSendCode.Size = new System.Drawing.Size(75, 23);
            this.buttonSendCode.TabIndex = 2;
            this.buttonSendCode.Text = "Send";
            this.buttonSendCode.UseVisualStyleBackColor = true;
            this.buttonSendCode.Click += new System.EventHandler(this.buttonSendCode_Click);
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(47, 13);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(100, 20);
            this.textBoxCode.TabIndex = 1;
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(6, 16);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(35, 13);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code:";
            // 
            // groupBoxConnect
            // 
            this.groupBoxConnect.Controls.Add(this.comboBoxRoomId);
            this.groupBoxConnect.Controls.Add(this.buttonConnect);
            this.groupBoxConnect.Controls.Add(this.labelRoomId);
            this.groupBoxConnect.Location = new System.Drawing.Point(8, 114);
            this.groupBoxConnect.Name = "groupBoxConnect";
            this.groupBoxConnect.Size = new System.Drawing.Size(243, 70);
            this.groupBoxConnect.TabIndex = 5;
            this.groupBoxConnect.TabStop = false;
            this.groupBoxConnect.Text = "Connect";
            // 
            // comboBoxRoomId
            // 
            this.comboBoxRoomId.FormattingEnabled = true;
            this.comboBoxRoomId.Location = new System.Drawing.Point(68, 13);
            this.comboBoxRoomId.Name = "comboBoxRoomId";
            this.comboBoxRoomId.Size = new System.Drawing.Size(169, 21);
            this.comboBoxRoomId.TabIndex = 7;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(162, 39);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelRoomId
            // 
            this.labelRoomId.AutoSize = true;
            this.labelRoomId.Location = new System.Drawing.Point(6, 16);
            this.labelRoomId.Name = "labelRoomId";
            this.labelRoomId.Size = new System.Drawing.Size(52, 13);
            this.labelRoomId.TabIndex = 0;
            this.labelRoomId.Text = "Room ID:";
            // 
            // groupBoxLogin
            // 
            this.groupBoxLogin.Controls.Add(this.textBoxPassword);
            this.groupBoxLogin.Controls.Add(this.comboBoxEmail);
            this.groupBoxLogin.Controls.Add(this.labelPassword);
            this.groupBoxLogin.Controls.Add(this.labelEmail);
            this.groupBoxLogin.Controls.Add(this.buttonLogin);
            this.groupBoxLogin.Location = new System.Drawing.Point(8, 6);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(243, 102);
            this.groupBoxLogin.TabIndex = 4;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Login";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(68, 45);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(169, 20);
            this.textBoxPassword.TabIndex = 6;
            // 
            // comboBoxEmail
            // 
            this.comboBoxEmail.FormattingEnabled = true;
            this.comboBoxEmail.Location = new System.Drawing.Point(68, 19);
            this.comboBoxEmail.Name = "comboBoxEmail";
            this.comboBoxEmail.Size = new System.Drawing.Size(169, 21);
            this.comboBoxEmail.TabIndex = 5;
            this.comboBoxEmail.SelectionChangeCommitted += new System.EventHandler(this.comboBoxEmail_SelectionChangeCommitted);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(6, 48);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(27, 22);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 3;
            this.labelEmail.Text = "Email:";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(162, 73);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // tabPageMinimap
            // 
            this.tabPageMinimap.Controls.Add(this.flowLayoutPanelMinimap);
            this.tabPageMinimap.Location = new System.Drawing.Point(4, 22);
            this.tabPageMinimap.Name = "tabPageMinimap";
            this.tabPageMinimap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMinimap.Size = new System.Drawing.Size(476, 430);
            this.tabPageMinimap.TabIndex = 1;
            this.tabPageMinimap.Text = "Minimap";
            this.tabPageMinimap.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelMinimap
            // 
            this.flowLayoutPanelMinimap.BackColor = System.Drawing.Color.Black;
            this.flowLayoutPanelMinimap.Controls.Add(this.pictureBoxMinimap);
            this.flowLayoutPanelMinimap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelMinimap.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelMinimap.Name = "flowLayoutPanelMinimap";
            this.flowLayoutPanelMinimap.Size = new System.Drawing.Size(470, 424);
            this.flowLayoutPanelMinimap.TabIndex = 0;
            // 
            // pictureBoxMinimap
            // 
            this.pictureBoxMinimap.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxMinimap.Name = "pictureBoxMinimap";
            this.pictureBoxMinimap.Size = new System.Drawing.Size(0, 0);
            this.pictureBoxMinimap.TabIndex = 0;
            this.pictureBoxMinimap.TabStop = false;
            // 
            // tabPageSubBots
            // 
            this.tabPageSubBots.Controls.Add(this.tabControlSubBots);
            this.tabPageSubBots.Location = new System.Drawing.Point(4, 22);
            this.tabPageSubBots.Name = "tabPageSubBots";
            this.tabPageSubBots.Size = new System.Drawing.Size(476, 430);
            this.tabPageSubBots.TabIndex = 3;
            this.tabPageSubBots.Text = "Subbots";
            this.tabPageSubBots.UseVisualStyleBackColor = true;
            // 
            // tabControlSubBots
            // 
            this.tabControlSubBots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSubBots.Location = new System.Drawing.Point(0, 0);
            this.tabControlSubBots.Name = "tabControlSubBots";
            this.tabControlSubBots.SelectedIndex = 0;
            this.tabControlSubBots.Size = new System.Drawing.Size(476, 430);
            this.tabControlSubBots.TabIndex = 0;
            // 
            // RtbConsoleInput
            // 
            this.RtbConsoleInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RtbConsoleInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RtbConsoleInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RtbConsoleInput.ForeColor = System.Drawing.Color.White;
            this.RtbConsoleInput.Location = new System.Drawing.Point(0, 439);
            this.RtbConsoleInput.Margin = new System.Windows.Forms.Padding(0);
            this.RtbConsoleInput.Name = "RtbConsoleInput";
            this.RtbConsoleInput.Size = new System.Drawing.Size(224, 13);
            this.RtbConsoleInput.TabIndex = 5;
            this.RtbConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtbConsoleInput_KeyDown);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tabControlMainTabs, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(720, 462);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(493, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 456);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.RtbConsole, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.RtbConsoleInput, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(220, 452);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // RtbConsole
            // 
            this.RtbConsole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.RtbConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RtbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RtbConsole.ForeColor = System.Drawing.Color.White;
            this.RtbConsole.Location = new System.Drawing.Point(0, 0);
            this.RtbConsole.Margin = new System.Windows.Forms.Padding(0);
            this.RtbConsole.Name = "RtbConsole";
            this.RtbConsole.ReadOnly = true;
            this.RtbConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.RtbConsole.Size = new System.Drawing.Size(224, 439);
            this.RtbConsole.TabIndex = 3;
            this.RtbConsole.Text = "";
            // 
            // tabPageEnableDisableSubBots
            // 
            this.tabPageEnableDisableSubBots.Controls.Add(this.groupBoxSubBots);
            this.tabPageEnableDisableSubBots.Location = new System.Drawing.Point(4, 22);
            this.tabPageEnableDisableSubBots.Name = "tabPageEnableDisableSubBots";
            this.tabPageEnableDisableSubBots.Size = new System.Drawing.Size(476, 430);
            this.tabPageEnableDisableSubBots.TabIndex = 4;
            this.tabPageEnableDisableSubBots.Text = "Enable/Disable Subbots";
            this.tabPageEnableDisableSubBots.UseVisualStyleBackColor = true;
            // 
            // groupBoxSubBots
            // 
            this.groupBoxSubBots.Controls.Add(this.checkedListBoxSubBots);
            this.groupBoxSubBots.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxSubBots.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSubBots.Name = "groupBoxSubBots";
            this.groupBoxSubBots.Size = new System.Drawing.Size(200, 430);
            this.groupBoxSubBots.TabIndex = 10;
            this.groupBoxSubBots.TabStop = false;
            this.groupBoxSubBots.Text = "Subbots";
            // 
            // checkedListBoxSubBots
            // 
            this.checkedListBoxSubBots.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxSubBots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxSubBots.FormattingEnabled = true;
            this.checkedListBoxSubBots.Location = new System.Drawing.Point(3, 16);
            this.checkedListBoxSubBots.Name = "checkedListBoxSubBots";
            this.checkedListBoxSubBots.Size = new System.Drawing.Size(194, 411);
            this.checkedListBoxSubBots.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 462);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "MainForm";
            this.Text = "MasterBot";
            this.tabControlMainTabs.ResumeLayout(false);
            this.tabPageLoginConnect.ResumeLayout(false);
            this.groupBoxCode.ResumeLayout(false);
            this.groupBoxCode.PerformLayout();
            this.groupBoxConnect.ResumeLayout(false);
            this.groupBoxConnect.PerformLayout();
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.tabPageMinimap.ResumeLayout(false);
            this.flowLayoutPanelMinimap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMinimap)).EndInit();
            this.tabPageSubBots.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPageEnableDisableSubBots.ResumeLayout(false);
            this.groupBoxSubBots.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMainTabs;
        private System.Windows.Forms.TabPage tabPageLoginConnect;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.GroupBox groupBoxConnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelRoomId;
        private Gui.RickTextConsole RtbConsole;
        private System.Windows.Forms.TextBox RtbConsoleInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabPage tabPageMinimap;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMinimap;
        private System.Windows.Forms.PictureBox pictureBoxMinimap;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxCode;
        private System.Windows.Forms.Button buttonSendCode;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.TabPage tabPageSubBots;
        private System.Windows.Forms.TabControl tabControlSubBots;
        private System.Windows.Forms.ComboBox comboBoxRoomId;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.ComboBox comboBoxEmail;
        private System.Windows.Forms.TabPage tabPageEnableDisableSubBots;
        private System.Windows.Forms.GroupBox groupBoxSubBots;
        private System.Windows.Forms.CheckedListBox checkedListBoxSubBots;
    }
}

