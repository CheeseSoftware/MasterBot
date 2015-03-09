using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot
{
    public static class SafeInvoke
    {
        public static void Invoke(this Control uiElement, Action updater, bool forceSynchronous = true)
        {
            if (uiElement == null)
            {
                throw new ArgumentNullException("uiElement");
            }

            if (uiElement.InvokeRequired)
            {
                if (forceSynchronous)
                {
                    try
                    {
                        uiElement.Invoke((Action)delegate { Invoke(uiElement, updater, forceSynchronous); });
                    }
                    catch (Exception e) { }
                }
                else
                {
                    uiElement.BeginInvoke((Action)delegate { Invoke(uiElement, updater, forceSynchronous); });
                }
            }
            else
            {
                if (!uiElement.IsDisposed)
                {
                    updater();
                }
            }
        }
    }
}
