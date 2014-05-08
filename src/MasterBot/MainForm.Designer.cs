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
            this.tabPageConnectLogin = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBoxConnect = new System.Windows.Forms.GroupBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxRoomId = new System.Windows.Forms.TextBox();
            this.labelRoomId = new System.Windows.Forms.Label();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RtbConsoleInput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RtbConsole = new Gui.RickTextConsole();
            this.tabControlMainTabs.SuspendLayout();
            this.tabPageConnectLogin.SuspendLayout();
            this.groupBoxConnect.SuspendLayout();
            this.groupBoxLogin.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMainTabs
            // 
            this.tabControlMainTabs.Controls.Add(this.tabPageConnectLogin);
            this.tabControlMainTabs.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tabControlMainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMainTabs.Location = new System.Drawing.Point(3, 3);
            this.tabControlMainTabs.Name = "tabControlMainTabs";
            this.tabControlMainTabs.SelectedIndex = 0;
            this.tabControlMainTabs.Size = new System.Drawing.Size(484, 456);
            this.tabControlMainTabs.TabIndex = 0;
            // 
            // tabPageConnectLogin
            // 
            this.tabPageConnectLogin.Controls.Add(this.button1);
            this.tabPageConnectLogin.Controls.Add(this.groupBoxConnect);
            this.tabPageConnectLogin.Controls.Add(this.groupBoxLogin);
            this.tabPageConnectLogin.Location = new System.Drawing.Point(4, 22);
            this.tabPageConnectLogin.Name = "tabPageConnectLogin";
            this.tabPageConnectLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConnectLogin.Size = new System.Drawing.Size(476, 430);
            this.tabPageConnectLogin.TabIndex = 0;
            this.tabPageConnectLogin.Text = "ConnectLogin";
            this.tabPageConnectLogin.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBoxConnect
            // 
            this.groupBoxConnect.Controls.Add(this.buttonConnect);
            this.groupBoxConnect.Controls.Add(this.textBoxRoomId);
            this.groupBoxConnect.Controls.Add(this.labelRoomId);
            this.groupBoxConnect.Location = new System.Drawing.Point(8, 114);
            this.groupBoxConnect.Name = "groupBoxConnect";
            this.groupBoxConnect.Size = new System.Drawing.Size(243, 70);
            this.groupBoxConnect.TabIndex = 5;
            this.groupBoxConnect.TabStop = false;
            this.groupBoxConnect.Text = "Connect";
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
            // textBoxRoomId
            // 
            this.textBoxRoomId.Location = new System.Drawing.Point(70, 13);
            this.textBoxRoomId.Name = "textBoxRoomId";
            this.textBoxRoomId.Size = new System.Drawing.Size(167, 20);
            this.textBoxRoomId.TabIndex = 1;
            this.textBoxRoomId.Text = "PWO0ktzRcQbkI";
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
            this.groupBoxLogin.Controls.Add(this.labelPassword);
            this.groupBoxLogin.Controls.Add(this.labelEmail);
            this.groupBoxLogin.Controls.Add(this.textBoxEmail);
            this.groupBoxLogin.Controls.Add(this.textBoxPassword);
            this.groupBoxLogin.Controls.Add(this.buttonLogin);
            this.groupBoxLogin.Location = new System.Drawing.Point(8, 6);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(243, 102);
            this.groupBoxLogin.TabIndex = 4;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Login";
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
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(70, 19);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(167, 20);
            this.textBoxEmail.TabIndex = 1;
            this.textBoxEmail.Text = "gustav9797@hotmail.se";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(70, 45);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(167, 20);
            this.textBoxPassword.TabIndex = 2;
            this.textBoxPassword.Text = "kasekakorna";
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.RtbConsoleInput, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(493, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(224, 456);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.RtbConsole);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(218, 430);
            this.panel1.TabIndex = 4;
            // 
            // RtbConsoleInput
            // 
            this.RtbConsoleInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.RtbConsoleInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.RtbConsoleInput.ForeColor = System.Drawing.Color.White;
            this.RtbConsoleInput.Location = new System.Drawing.Point(3, 436);
            this.RtbConsoleInput.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.RtbConsoleInput.Name = "RtbConsoleInput";
            this.RtbConsoleInput.Size = new System.Drawing.Size(218, 20);
            this.RtbConsoleInput.TabIndex = 5;
            this.RtbConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtbConsoleInput_KeyDown);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tabControlMainTabs, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(720, 462);
            this.tableLayoutPanel2.TabIndex = 4;
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
            this.RtbConsole.Size = new System.Drawing.Size(214, 426);
            this.RtbConsole.TabIndex = 3;
            this.RtbConsole.Text = "";
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
            this.tabPageConnectLogin.ResumeLayout(false);
            this.groupBoxConnect.ResumeLayout(false);
            this.groupBoxConnect.PerformLayout();
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMainTabs;
        private System.Windows.Forms.TabPage tabPageConnectLogin;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.GroupBox groupBoxConnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxRoomId;
        private System.Windows.Forms.Label labelRoomId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private Gui.RickTextConsole RtbConsole;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox RtbConsoleInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}

