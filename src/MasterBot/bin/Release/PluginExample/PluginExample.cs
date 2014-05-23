using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot;
using MasterBot.SubBot;
using MasterBot.Room;
using System.Threading;
using MasterBot.Room.Block;
using System.Windows.Forms;

namespace PluginExample
{
    // Don't forget GUI!
    // Note that the SubBot and the Plugin can be seperated.
    public class PluginExample : ASubBot, IPlugin
    {
        private IBlockDrawer blockDrawer;

        private NumericUpDown nudBlockId;
        private TextBox textBox1;
        private CheckBox cbDisableEdit;
        private Button btnFill;
    
        public PluginExample() : base(null)
        {
        }

        public string PluginName
        {
            get { return "Plugin Example 1"; }
        }

        public override bool HasTab
        {
            // This makes the tab visible.
            get { return true; }
        }

        public override string SubBotName
        {
            get { return "Plugin_Example_1"; }
        }

        public void PerformAction(IBot bot)
        {
            // This is required since I can't fix this in the constructor. :/
            this.bot = bot;

            // 0 is the Priority, it's the lowest possible.
            blockDrawer = bot.Room.BlockDrawerPool.CreateBlockDrawer(0);

            // Starts the BlockDrawer.
            blockDrawer.Start();

            // Adds the SubBot to the SubBot handler so the SubBot will get callbacks.
            bot.SubBotHandler.AddSubBot(this, true);
        }

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmd == "test")
                cmdSource.Reply("Plugin Example 1 is working!");
        }

        public override void onBlockChange(int x, int y, MasterBot.Room.Block.IBlock newBlock, MasterBot.Room.Block.IBlock oldBlock)
        {
            if (cbDisableEdit.Checked)
            {
                if (newBlock.Id != (int)nudBlockId.Value)
                {
                    // This uses the public BlockDrawer.
                    bot.Room.setBlock(x, y,
                        new NormalBlock((int)nudBlockId.Value));

                    // This does the same thing as above.
                    //bot.Room.BlockDrawer.PlaceBlock(new BlockWithPos(
                    //    x, y,
                    //    new NormalBlock((int)nudBlockId.Value)
                    //    ));

                }
            }
        }

        public override void onTick()
        {
        }

        protected override void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginExample));
            this.btnFill = new System.Windows.Forms.Button();
            this.nudBlockId = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbDisableEdit = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlockId)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFill
            // 
            this.btnFill.Location = new System.Drawing.Point(3, 32);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(75, 23);
            this.btnFill.TabIndex = 0;
            this.btnFill.Text = "Fill";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            // 
            // nudBlockId
            // 
            this.nudBlockId.Location = new System.Drawing.Point(3, 6);
            this.nudBlockId.Name = "nudBlockId";
            this.nudBlockId.Size = new System.Drawing.Size(75, 20);
            this.nudBlockId.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 110);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(250, 90);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // cbDisableEdit
            // 
            this.cbDisableEdit.AutoSize = true;
            this.cbDisableEdit.Location = new System.Drawing.Point(3, 87);
            this.cbDisableEdit.Name = "cbDisableEdit";
            this.cbDisableEdit.Size = new System.Drawing.Size(180, 17);
            this.cbDisableEdit.TabIndex = 3;
            this.cbDisableEdit.Text = "disable edit without blockdrawer.";
            this.cbDisableEdit.UseVisualStyleBackColor = true;
            // 
            // PluginExample
            // 
            this.Controls.Add(this.cbDisableEdit);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.nudBlockId);
            this.Controls.Add(this.btnFill);
            this.Name = "PluginExample";
            this.Size = new System.Drawing.Size(250, 200);
            ((System.ComponentModel.ISupportInitialize)(this.nudBlockId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            int id = (int)nudBlockId.Value;

            for (int x = 1; x < bot.Room.Width - 1; x++)
            {
                for (int y = 1; y < bot.Room.Height - 1; y++)
                {
                    // This method places the block. Use this and not DrawBlock().
                    blockDrawer.PlaceBlock(new BlockWithPos(x, y, new NormalBlock(id)));
                }
            }
        }



    }
}
