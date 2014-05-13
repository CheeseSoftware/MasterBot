using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MasterBot
{
    public interface IRichTextConsole
    {
        RichTextBox RickTextBox { get; }

        /// <summary>
        /// % - special character.
        /// %% -> %
        /// %s,%S -> Standard colors and font
        /// %e,%E -> Error font!
        /// %c -> Foreground color
        /// %C -> Background color
        /// %B -> Bold
        /// %b -> Not bold
        /// %I -> Italic
        /// %i -> Not italic
        /// %U -> Underline
        /// %u -> Not underline
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color">The colors for %c and %C.</param>
        void WriteLine(string text, params Color[] color);
    }
}
