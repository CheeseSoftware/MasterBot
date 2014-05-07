using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    class RickTextConsole : RichTextBox, MasterBot.IRichTextConsole
    {
        public System.Windows.Forms.RichTextBox RickTextBox
        {
            get { return this; }
        }

        // % - special character.
        // %% -> %
        // %s,%S -> Standard colors and font
        // %e,%E -> Error font!
        // %c -> Foreground color
        // %C -> Background color
        // %B -> Bold
        // %b -> Not bold
        // %I -> Italic
        // %i -> Not italic
        // %U -> Underline
        // %u -> Not underline
        public void WriteLine(string text, params Color[] color)
        {
            this.Invoke(new Action(() =>
                {
                    int startIndex = 0;
                    int colorIndex = 0;

                    int cStart = -1, CStart = -1, bStart = -1, iStart = -1, uStart = -1;
                    Color cColor = Color.White;
                    Color CColor = Color.FromArgb(32, 32, 32);

                    #region lambda
                    Action cEnd = () =>
                    {
                        if (cStart != -1)
                        {
                            this.Select(cStart, this.Text.Length - cStart);
                            this.SelectionColor = cColor;
                        }
                    };
                    Action CEnd = () =>
                    {
                        if (CStart != -1)
                        {
                            this.Select(CStart, this.Text.Length - CStart);
                            this.SelectionBackColor = CColor;
                        }
                    };
                    Action bEnd = () =>
                    {
                        if (bStart != -1)
                        {
                            this.Select(bStart, this.Text.Length - bStart);
                            this.SelectionFont = new Font(this.SelectionFont, FontStyle.Bold);
                        }
                    };
                    Action iEnd = () =>
                    {
                        if (iStart != -1)
                        {
                            this.Select(iStart, this.Text.Length - iStart);
                            this.SelectionFont = new Font(this.SelectionFont, FontStyle.Italic);
                        }
                    };
                    Action uEnd = () =>
                    {
                        if (iStart != -1)
                        {
                            this.Select(iStart, this.Text.Length - iStart);
                            this.SelectionFont = new Font(this.SelectionFont, FontStyle.Underline);
                        }
                    };
                    #endregion

                    for (int i = 1; i < text.Length; i++)
                    {
                        if (text[i - 1] == '%')
                        {
                            if (text[i] == '%')
                            {
                                this.AppendText("%");
                                startIndex = i + 1;
                            }
                            else
                            {
                                if (i - startIndex >= 2)
                                    RickTextBox.AppendText(text.Substring(startIndex, i - 1 - startIndex));

                                //string RtfText
#region switch...
                                switch (text[i])
                                {
                                    case 's':
                                    case 'S':
                                        cEnd();
                                        CEnd();
                                        bEnd();
                                        iEnd();
                                        uEnd();
                                        break;
                                    case 'e':
                                    case 'E':
                                        cEnd();
                                        CEnd();
                                        bEnd();

                                        cStart = this.Text.Length;
                                        CStart = this.Text.Length;
                                        bStart = this.Text.Length;

                                        cColor = color[colorIndex];
                                        colorIndex = (colorIndex + 1) % color.Length;

                                        CColor = color[colorIndex];
                                        colorIndex = (colorIndex + 1) % color.Length;
                                        break;
                                    case 'c':
                                        cEnd();
                                        cStart = this.Text.Length;
                                        cColor = color[colorIndex];
                                        colorIndex = (colorIndex + 1) % color.Length;
                                        break;
                                    case 'C':
                                        CEnd();
                                        cStart = this.Text.Length;
                                        CColor = color[colorIndex];
                                        colorIndex = (colorIndex + 1) % color.Length;
                                        break;
                                    case 'B':
                                        bEnd();
                                        bStart = this.Text.Length;
                                        break;
                                    case 'b':
                                        bEnd();
                                        break;
                                    case 'I':
                                        iEnd();
                                        iStart = this.Text.Length;
                                        break;
                                    case 'i':
                                        iEnd();
                                        break;
                                    case 'U':
                                        uEnd();
                                        uStart = this.Text.Length;
                                        break;
                                    case 'u':
                                        uEnd();
                                        break;

                                    default:
                                        this.AppendText("[%" + text[i] + "]");
                                        break;
                                }
#endregion

                                startIndex = i + 1;
                                continue;
                            }
                        }


                    }

                    this.AppendText(text.Substring(startIndex, text.Length - startIndex));
                    this.AppendText(Environment.NewLine);

                    CEnd();
                    cEnd();
                    bEnd();
                    iEnd();
                    uEnd();

                    this.Select(this.Text.Length, 0);
                    this.ScrollToCaret();

                }));
        }
    }
}
