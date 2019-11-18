using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace texteditor1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolStrip1.Renderer = new ToolStripStripeRemoval();

        }
        public int line = 0;
        public int column = 0;
        public static Boolean isCurslyBracesKeyPressed = false;
        public class ToolStripStripeRemoval : ToolStripSystemRenderer
        {
            public ToolStripStripeRemoval() { }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // ToolStripStripeRemoval
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "My open file dialog";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();
                using (StreamReader sr = new StreamReader(openfile.FileName))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save file as..";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter txtoutput = new StreamWriter(savefile.FileName);
                txtoutput.Write(richTextBox1.Text);
                txtoutput.Close();
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void salectAllButton_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripButton.PerformClick();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripButton.PerformClick();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton.PerformClick();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            undoButton.PerformClick();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            redoButton.PerformClick();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutToolStripButton.PerformClick();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripButton.PerformClick();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            pasteToolStripButton.PerformClick();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAllButton.PerformClick();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            line = 1 + richTextBox1.GetLineFromCharIndex(richTextBox1.GetFirstCharIndexOfCurrentLine());
            column = 1 + richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexOfCurrentLine();
            toolStripStatusLabel1.Text = "line: " + line.ToString() + " | column: " + column.ToString();
        }

        private void jumpToTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.ScrollToCaret();
        }

        private void jumpToBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keywords = textBox1.Text;
            MatchCollection keywordMatches = Regex.Matches(richTextBox1.Text, keywords);

            int originalIndex = richTextBox1.SelectionStart;
            int originalLength = richTextBox1.SelectionLength;

            // Stops blinking
            textBox1.Focus();

            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionBackColor = Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));

            foreach (Match m in keywordMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.White;
                richTextBox1.SelectionBackColor = Color.DodgerBlue;
            }

            richTextBox1.SelectionStart = originalIndex;
            richTextBox1.SelectionLength = originalLength;
            richTextBox1.SelectionBackColor = Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null && !string.IsNullOrWhiteSpace(textBox2.Text) && textBox3.Text != null && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                richTextBox1.Text = richTextBox1.Text.Replace(textBox2.Text, textBox3.Text);
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void findAndReplacceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {


            // getting keywords/functions
            string keywords = @"\b(public|private|partial|static|namespace|class|using|void|foreach|in)\b";
            MatchCollection keywordMatches = Regex.Matches(richTextBox1.Text, keywords);

            // getting types/classes from the text 
            string types = @"\b(Console)\b";
            MatchCollection typeMatches = Regex.Matches(richTextBox1.Text, types);

            // getting comments (inline or multiline)
            string comments = @"(\/\/.+?$|\/\*.+?\*\/)";
            MatchCollection commentMatches = Regex.Matches(richTextBox1.Text, comments, RegexOptions.Multiline);

            // getting strings
            string strings = "\".+?\"";
            MatchCollection stringMatches = Regex.Matches(richTextBox1.Text, strings);

            // saving the original caret position + forecolor
            int originalIndex = richTextBox1.SelectionStart;
            int originalLength = richTextBox1.SelectionLength;
            Color originalColor = Color.Black;

            // MANDATORY - focuses a label before highlighting (avoids blinking)
            // titleLabel.Focus();

            // removes any previous highlighting (so modified words won't remain highlighted)
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = originalColor;

            // scanning...
            foreach (Match m in keywordMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Blue;
            }

            foreach (Match m in typeMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.DarkCyan;
            }

            foreach (Match m in commentMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Green;
            }

            foreach (Match m in stringMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Red;
            }

            // restoring the original colors, for further writing
            richTextBox1.SelectionStart = originalIndex;
            richTextBox1.SelectionLength = originalLength;
            richTextBox1.SelectionColor = originalColor;

            // giving back the focus
            richTextBox1.Focus();



        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            String s = e.KeyChar.ToString();
            int sel = richTextBox1.SelectionStart;
            if (checkBox1.Checked == true)
            {
                switch (s)
                {
                    case "(":
                        richTextBox1.Text = richTextBox1.Text.Insert(sel, "()");
                        e.Handled = true;
                        richTextBox1.SelectionStart = sel + 1;
                        break;

                    case "{":
                        String t = "{}";
                        richTextBox1.Text = richTextBox1.Text.Insert(sel, t);
                        e.Handled = true;
                        richTextBox1.SelectionStart = sel + t.Length - 1;
                        isCurslyBracesKeyPressed = true;
                        break;

                    case "[":
                        richTextBox1.Text = richTextBox1.Text.Insert(sel, "[]");
                        e.Handled = true;
                        richTextBox1.SelectionStart = sel + 1;
                        break;

                    case "<":
                        richTextBox1.Text = richTextBox1.Text.Insert(sel, "<>");
                        e.Handled = true;
                        richTextBox1.SelectionStart = sel + 1;
                        break;

                    case "\"":
                        richTextBox1.Text = richTextBox1.Text.Insert(sel, "\"\"");
                        e.Handled = true;
                        richTextBox1.SelectionStart = sel + 1;
                        break;

                    case "'":
                        richTextBox1.Text = richTextBox1.Text.Insert(sel, "''");
                        e.Handled = true;
                        richTextBox1.SelectionStart = sel + 1;
                        break;
                }
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int sel = richTextBox1.SelectionStart;
            if (e.KeyCode == Keys.Enter)
            {
                if (isCurslyBracesKeyPressed == true)
                {
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "\n          \n");
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + "          ".Length;
                    isCurslyBracesKeyPressed = false;
                }
            }
        }
    }
}
    


