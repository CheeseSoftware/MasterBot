using MasterBot;
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
            SafeInvoke.Invoke(this, new Action(() =>
                {
                    int startIndex = 0;
                    int colorIndex = 0;

                    int fontStart = this.Text.Length;
                    int cStart = this.Text.Length;
                    int CStart = this.Text.Length;
                    Color cColor = Color.White;
                    Color CColor = Color.FromArgb(32, 32, 32);
                    FontStyle fontStyle = FontStyle.Regular;



                    #region lambda
                    Action EndFont = () =>
                    {
                        this.Select(fontStart, this.Text.Length - fontStart);
                        this.SelectionFont = new Font(this.SelectionFont, fontStyle);
                        fontStart = this.Text.Length;
                    };

                    Action cEnd = () =>
                    {
                        if (cStart != -1)
                        {
                            this.Select(cStart, this.Text.Length - cStart);
                            this.SelectionColor = cColor;
                            cStart = this.Text.Length;
                        }
                    };

                    Action CEnd = () =>
                    {
                        if (CStart != -1)
                        {
                            this.Select(CStart, this.Text.Length - CStart);
                            this.SelectionBackColor = CColor;
                            CStart = this.Text.Length;
                        }
                    };
                    #endregion

                    for (int i = 1; i < text.Length; i++)
                    {
                        if (text[i - 1] == '%')
                        {
                            if (text[i] == '%')
                            {
                                RickTextBox.AppendText(text.Substring(startIndex, i - 1 - startIndex));
                                this.AppendText("%");
                                startIndex = i + 1;
                                i++;
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
                                        EndFont();
                                        fontStyle = FontStyle.Regular;
                                        cColor = Color.White;
                                        CColor = Color.FromArgb(32, 32, 32);
                                        break;
                                    case 'e':
                                    case 'E':
                                        cEnd();
                                        CEnd();
                                        EndFont();
                                        fontStyle = FontStyle.Bold;

                                        cStart = this.Text.Length;
                                        CStart = this.Text.Length;

                                        cColor = Color.Red;
                                        CColor = Color.Black;
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
                                        EndFont();
                                        fontStyle |= FontStyle.Bold;
                                        //BEnd();
                                        //BStart(); // = this.Text.Length;
                                        break;
                                    case 'b':
                                        EndFont();
                                        fontStyle &= ~FontStyle.Bold;
                                        break;
                                    case 'I':
                                        EndFont();
                                        fontStyle |= FontStyle.Italic;
                                        break;
                                    case 'i':
                                        EndFont();
                                        fontStyle &= ~FontStyle.Italic;
                                        break;
                                    case 'U':
                                        EndFont();
                                        fontStyle |= FontStyle.Underline;
                                        break;
                                    case 'u':
                                        EndFont();
                                        fontStyle &= ~FontStyle.Underline;
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
                    EndFont();

                    this.Select(this.Text.Length, 0);

                    this.ScrollToCaret();

                }));
        }
    }
}
