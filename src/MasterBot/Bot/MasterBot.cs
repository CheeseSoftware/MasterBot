using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasterBot.SubBot;
using MasterBot.Room;
using PlayerIOClient;
using MasterBot.Movement;
using MasterBot.Minimap;
using MasterBot.SubBot.WorldEdit;
using System.Threading;
using System.IO;
using System.Xml;
using System.Reflection;

namespace MasterBot
{
    public class MasterBot : IBot
    {
        private MainForm mainForm;
        private SubBotHandler subBotHandler;
        private Client client;
        private Connection connection;
        private IRoom room;
        private ChatSayer chatSayer = null;

        public bool LoggedIn { get { return client != null; } }
        public bool Connected { get { return connection != null && connection.Connected; } }

        public MasterBot()
        {
            MinimapColors.CreateColorCodes();

            mainForm = new MainForm(this);
            mainForm.FormClosing += delegate
            {
                Disconnect("Form Closing");
            };
            new Thread(() => { Application.Run(mainForm); }).Start();

            subBotHandler = new SubBotHandler(this, mainForm.BotTabPage);
            subBotHandler.AddSubBot((ASubBot)(room = new Room.Room(this)));
            subBotHandler.AddSubBot(new BlockPlaceTest(this));
            subBotHandler.AddSubBot(new Commands(this));
            subBotHandler.AddSubBot(new WorldEdit(this));
            subBotHandler.AddSubBot(new Protection(this));


            //////////////////////////////////////

            //PluginSectionHandler pluginSectionHandler = new PluginSectionHandler();

            Action<string[]> lambda = new Action<string[]>((string[] pluginFiles) =>
                {
                    for (int j = 0; j < pluginFiles.Count(); j++)
                    {
                        if (Path.GetFileName(pluginFiles[j]).EndsWith(".dll")
                            && Path.GetFileName(pluginFiles[j]) != "PlayerIOClient.dll"
                            && Path.GetFileName(pluginFiles[j]) != "MasterBotLib.dll")
                        {
                            Assembly.LoadFrom(Path.GetFullPath(pluginFiles[j]));

                            //Loop through all opened assemblies in the current AppDomain
                            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                            {
                                foreach (Type t in a.GetTypes())
                                {
                                    if (t.GetInterface("IPlugin") != null)
                                    {
                                        try
                                        {
                                            IPlugin pluginclass = Activator.CreateInstance(t) as IPlugin;
                                            pluginclass.PerformAction(this);
                                            break;
                                        }
                                        catch (Exception e)
                                        {
                                            this.mainForm.Console.WriteLine("%eError loading plugin(may be false): " + e.Message);
                                        }
                                    }
                                }
                            }

                            /////////////////////////////////////////
                            //  OLD /////////////////////////////////
                            /////////////////////////////////////////

                            /*string text;
                            StreamReader reader = new StreamReader(pluginFiles[j]);
                            text = reader.ReadToEnd();
                            reader.Close();

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(text);

                            List<IPlugin> plugins = (List<IPlugin>) pluginSectionHandler.Create(this, pluginFiles[j], xmlDoc);

                            foreach(IPlugin p in plugins)
                                p.PerformAction(this);*/


                        }
                    }
                });

            const string path = "plugins\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            lambda(Directory.GetFiles(path));

            string[] pluginDirectories = Directory.GetDirectories("plugins\\");
            
            for (int i = 0; i < pluginDirectories.Count(); i++)
            {
                lambda(Directory.GetFiles(pluginDirectories[i]));
            }

        }

        public void onMessage(object sender, PlayerIOClient.Message m)
        {
            subBotHandler.onMessage(m);
        }

        public void onDisconnect(object sender, string reason)
        {
            Disconnect(reason);
        }

        public void Login(string game, string email, string password)
        {
            PlayerIO.QuickConnect.SimpleConnect(
                "everybody-edits-su9rn58o40itdbnw69plyw",
                email,
                password,
                delegate(Client tempClient)
                {
                    client = tempClient;
                    SafeInvoke.Invoke(mainForm, new Action(() => { mainForm.onLoginFinished(true); }));
                },
                delegate(PlayerIOError tempError)
                {
                    SafeInvoke.Invoke(mainForm, new Action(() => { mainForm.onLoginFinished(false); }));
                    MessageBox.Show(tempError.ToString());
                });
        }

        public bool Connect(string roomId)
        {
            if (LoggedIn)
            {
                client.Multiplayer.JoinRoom(
                    roomId,
                    null,
                    delegate(PlayerIOClient.Connection tempConnection)
                    {
                        connection = tempConnection;
                        connection.Send("init");
                        connection.AddOnDisconnect(new DisconnectEventHandler(onDisconnect));
                        connection.AddOnMessage(new MessageReceivedEventHandler(onMessage));

                        chatSayer = new ChatSayer(this);

                        subBotHandler.onConnect();
                        
                        //this fails!
                        SafeInvoke.Invoke(mainForm, new Action(() => { mainForm.onConnectFinished(true); }));
                        

                    },
                    delegate(PlayerIOError tempError)
                    {
                        SafeInvoke.Invoke(mainForm, new Action(() => { mainForm.onConnectFinished(false); }));
                        MessageBox.Show(tempError.ToString());
                    });
                return true;
            }
            return false;
        }

        public void Disconnect(string reason)
        {
            if (Connected)
                connection.Disconnect();
            subBotHandler.onDisconnect(reason);
            lock (chatSayer)
            {
                chatSayer.onDisconnect();
                chatSayer = null;
            }
        }

        public ISubBotHandler SubBotHandler
        {
            get { return subBotHandler; }
        }

        public IMainForm MainForm
        {
            get { return mainForm; }
        }

        public Client Client
        {
            get { return client; }
        }

        public Connection Connection
        {
            get { return connection; }
        }

        public IRoom Room
        {
            get { return room; }
        }

        public void Say(string message)
        {
            lock (chatSayer)
            {
                if (chatSayer != null)
                    chatSayer.Say(message);
            }
        }
    }
}
