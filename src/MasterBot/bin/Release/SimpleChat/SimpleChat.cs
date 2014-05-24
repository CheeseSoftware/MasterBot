using MasterBot;
using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;
using System.Diagnostics;

namespace SimpleChat
{
    public class SimpleChat : ASubBot, IPlugin
    {
        private SimpleChatGui.RichTextConsoleChild rtcChatBox;
        private System.Windows.Forms.TextBox tbChatInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Stopwatch chatStopWatch = new Stopwatch();
    
        public override bool HasTab
        {
            get { return true; }
        }

        public override string SubBotName
        {
            get { return "SimpleChat"; }
        }

        public SimpleChat()
            : base(null)
        {
            chatStopWatch.Start();
        }

        public void PerformAction(IBot bot)
        {
            InitializeComponent();
            this.bot = bot;

            bot.SubBotHandler.AddSubBot(this, true);
        }

        public override void onBlockChange(int x, int y, MasterBot.Room.Block.IBlock newBlock, MasterBot.Room.Block.IBlock oldBlock)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
        }

        public override void onConnect()
        {
        }

        public override void onDisable()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onEnable()
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "info":
                    {
                        string title = m.GetString(0);
                        string message = m.GetString(1);

                        this.rtcChatBox.WriteLine("%c%B(Info) " + title + "%s: " + message.Replace("%", "%%"), Color.FromArgb(255, 192, 13));
                    }
                    break;
                case "write":
                    {
                        string title = m.GetString(0);
                        string message = m.GetString(1);

                        this.rtcChatBox.WriteLine("%B" + title + "%b: " + message.Replace("%", "%%"));
                    }
                    break;
                case "say":
                    {
                        int userId = m.GetInt(0);
                        string message = m.GetString(1);

                        IPlayer player = bot.Room.getPlayer(userId);

                        if (player != null)
                        {
                            this.rtcChatBox.WriteLine(player.Name + ": " + message.Replace("%", "%%"), Color.FromArgb(255, 0, 0));
                        }
                    }
                    break;
            }
        }

        public override void onTick()
        {
        }

        private void InitializeComponent()
        {
            this.rtcChatBox = new SimpleChatGui.RichTextConsoleChild();
            this.tbChatInput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtcChatBox
            // 
            this.rtcChatBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.rtcChatBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtcChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtcChatBox.ForeColor = System.Drawing.Color.White;
            this.rtcChatBox.Location = new System.Drawing.Point(0, 0);
            this.rtcChatBox.Margin = new System.Windows.Forms.Padding(0);
            this.rtcChatBox.Name = "rtcChatBox";
            this.rtcChatBox.ReadOnly = true;
            this.rtcChatBox.Size = new System.Drawing.Size(300, 287);
            this.rtcChatBox.TabIndex = 0;
            this.rtcChatBox.Text = "";
            // 
            // tbChatInput
            // 
            this.tbChatInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbChatInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbChatInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbChatInput.ForeColor = System.Drawing.Color.White;
            this.tbChatInput.Location = new System.Drawing.Point(0, 287);
            this.tbChatInput.Margin = new System.Windows.Forms.Padding(0);
            this.tbChatInput.Name = "tbChatInput";
            this.tbChatInput.Size = new System.Drawing.Size(300, 13);
            this.tbChatInput.TabIndex = 1;
            this.tbChatInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbChatInput_KeyDown);
            this.tbChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbChatInput_KeyPress);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbChatInput, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtcChatBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 300);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // SimpleChat
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SimpleChat";
            this.Size = new System.Drawing.Size(300, 300);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void tbChatInput_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter
                && chatStopWatch.ElapsedMilliseconds >= 1000)
            {
                // Use bot.Say. It queues the messages so they won't be blocked by anti-spam.
                bot.Say(tbChatInput.Text);
                this.rtcChatBox.WriteLine("%B(You): %b" + tbChatInput.Text);
                tbChatInput.Text = "";
                chatStopWatch.Restart();
            }
        }

        private void tbChatInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }

    }
}
