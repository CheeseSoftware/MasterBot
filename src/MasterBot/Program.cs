using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot
{
    static class Program
    {
        static MasterBot bot;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bot = new MasterBot();
#else
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                bot = new MasterBot();
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.ToString());
            }
#endif
        }
    }
}
