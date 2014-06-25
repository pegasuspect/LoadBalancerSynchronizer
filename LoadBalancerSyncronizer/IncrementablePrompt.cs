using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadBalancerSyncronizer;

namespace LoadBalancerSyncronizer
{
    using Cinar.Database;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public static class IncrementablePrompt
    {
        public static IncrementablePromptResult ShowDialog(string text, string caption, List<string> defaultInputValues = null)
        {
            Form prompt = new Form();

            init(caption, prompt);

            Label textLabel = new Label() { Left = 45, Top = 20, Text = text, Width = 400, Font = new System.Drawing.Font(Label.DefaultFont.FontFamily, (float)(Label.DefaultFont.Size * 1.5)) };
            prompt.Controls.Add(textLabel);

            Button btnConfirmation = new Button() { Text = "Ok", Left = 351, Width = 100, Top = 75 };
            btnConfirmation.Click += (sender, e) =>
            {
                prompt.DialogResult = DialogResult.OK;
                prompt.Close();
            };

            ListView list = new ListView() { Left = 50, Top = 50, Width = 400 };
            list.BorderStyle = BorderStyle.None;
            list.BackColor = prompt.BackColor;
            list.Height = 0;

            Button btnAdd = new Button() { Text = "+", Width = 20, Height = 20, Left = list.Left, Top = btnConfirmation.Top };
            Button btnRemove = new Button() { Text = "-", Width = 20, Height = 20, Left = list.Left + 30, Top = btnConfirmation.Top };

            btnAdd.Click += (sender, e) =>
            {
                list.addTextBox(prompt, btnAdd, btnConfirmation, btnRemove);
            };

            btnRemove.Click += (sender, e) =>
            {
                list.removeTextBox(prompt, btnAdd, btnConfirmation, btnRemove);
            };

            int i = 0;
            do
            {
                string defaultVal = defaultInputValues.Count == 0 ? "" : defaultInputValues[i];
                list.addTextBox(prompt, btnAdd, btnConfirmation, btnRemove, defaultVal);
                i++;
            }
            while (i < defaultInputValues.Count);


            prompt.Controls.Add(list);
            prompt.Controls.Add(btnConfirmation);
            prompt.Controls.Add(btnAdd);
            prompt.Controls.Add(btnRemove);


            prompt.AcceptButton = btnConfirmation;
            IncrementablePromptResult res = new IncrementablePromptResult();
            res.closeOperation = prompt.ShowDialog();
            res.inputValues = new List<string>();
            foreach (TextBox item in list.Controls)
            {
                if (!string.IsNullOrWhiteSpace(item.Text.Trim()))
                    res.inputValues.Add(item.Text);
            }
            return res;
        }

        private static void addTextBox(this ListView list, Form prmpt, Button add, Button conf, Button remove, string defVal = "")
        {
            list.BeginUpdate();
            TextBox txtBox = null;
            if (list.Controls.Count > 0)
            {
                txtBox = (TextBox)list.Controls[list.Controls.Count - 1];
            }
            int lastTxtBoxHeight = txtBox != null ? txtBox.Height + 10 : 0;
            int lastTxtBoxTop = txtBox != null ? txtBox.Top : 0;
            TextBox textBox = new TextBox() { Top = lastTxtBoxTop + lastTxtBoxHeight, Width = 400, Text = defVal };
            list.Controls.Add(textBox);
            list.Height += lastTxtBoxHeight == 0 ? 20 : lastTxtBoxHeight;
            prmpt.Height += lastTxtBoxHeight;
            add.Top += lastTxtBoxHeight;
            remove.Top += lastTxtBoxHeight;
            conf.Top += lastTxtBoxHeight;
            list.EndUpdate();
        }

        private static void removeTextBox(this ListView list, Form prmpt, Button add, Button conf, Button remove)
        {
            list.BeginUpdate();
            if (list.Controls.Count > 1)
            {
                TextBox txtBox = (TextBox)list.Controls[list.Controls.Count - 1];
                int lastTxtBoxHeight = txtBox.Height + 10;
                list.Height -= lastTxtBoxHeight;
                prmpt.Height -= lastTxtBoxHeight;
                add.Top -= lastTxtBoxHeight;
                remove.Top -= lastTxtBoxHeight;
                conf.Top -= lastTxtBoxHeight;
                list.Controls.RemoveAt(list.Controls.Count - 1);
            }

            list.EndUpdate();
        }

        private static void init(string caption, Form prompt)
        {
            prompt.Width = 500;
            prompt.Height = 170;
            prompt.Text = caption;
            prompt.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            prompt.StartPosition = FormStartPosition.CenterScreen;
        }

    }
    public class IncrementablePromptResult
    {
        public DialogResult closeOperation { get; set; }
        public List<string> inputValues { get; set; }
    }
}
