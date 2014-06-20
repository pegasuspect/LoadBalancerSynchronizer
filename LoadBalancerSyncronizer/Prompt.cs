using Cinar.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancerSyncronizer
{
    public static class Prompt
    {
        public static PromptResult ShowDialog(string text, string caption, bool isDBSettings = false, string defaultInputValue = "")
        {
            Form prompt = new Form();

            init(caption, prompt);

            Label textLabel = new Label() { Left = 45, Top = 20, Text = text, Width = 400, Font = new System.Drawing.Font(Label.DefaultFont.FontFamily, (float)(Label.DefaultFont.Size * 1.5)) };
            prompt.Controls.Add(textLabel);

            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            textBox.Text = defaultInputValue;
            prompt.Controls.Add(textBox);

            ComboBox cmb = new ComboBox() { Left = 50, Width = 250, Top = 75 };
            if (isDBSettings)
            {
                cmb.Text = "Select DB Type";
                foreach (DatabaseProvider dbType in Enum.GetValues(typeof(DatabaseProvider)))
                {
                    cmb.Items.Add(dbType);
                }
                cmb.SelectedItem = Form1.DATA.ConnectionType;
                prompt.Controls.Add(cmb);
            }

            Button confirmation = new Button() { Text = "Ok", Left = 351, Width = 100, Top = 75 };
            confirmation.Click += (sender, e) =>
            {
                prompt.DialogResult = DialogResult.OK;
                prompt.Close();
            };

            textBox.KeyPress += prompt_KeyPress;

            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            PromptResult res = new PromptResult();
            res.closeOperation = prompt.ShowDialog();
            res.inputValue = textBox.Text;
            if (isDBSettings)
            {
                res.DbConnectionType = cmb.SelectedItem == null ? DatabaseProvider.MySQL : (DatabaseProvider)cmb.SelectedItem;
            }

            return res;
        }

        private static void prompt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                Form.ActiveForm.Close();
            }
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

    public class PromptResult
    {
        public DialogResult closeOperation { get; set; }
        public string inputValue { get; set; }
        public DatabaseProvider DbConnectionType { get; set; }
    }
}
