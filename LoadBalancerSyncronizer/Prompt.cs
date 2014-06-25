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

            textBox.KeyPress += prompt_KeyPress;

            Button confirmation = new Button() { Text = "Ok", Left = 351, Width = 100, Top = 75 };
            confirmation.Click += (sender, e) =>
            {
                prompt.DialogResult = DialogResult.OK;
                prompt.Close();
            };

            #region Create Combo If Needed
            ComboBox cmb = new ComboBox();

            if (isDBSettings)
            {
                cmb.Left = 50;
                cmb.Width = 250;
                cmb.Top = 75;
                cmb.Text = "Select DB Type";

                foreach (DatabaseProvider dbType in Enum.GetValues(typeof(DatabaseProvider)))
                {
                    cmb.Items.Add(dbType);
                }

                cmb.SelectedItem = Form1.DATA.ConnectionType;

                prompt.Controls.Add(cmb);
            }
            #endregion

            prompt.Controls.Add(textBox);
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
