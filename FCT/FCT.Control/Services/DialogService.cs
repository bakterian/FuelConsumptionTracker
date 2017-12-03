using System;
using System.Windows.Forms;
using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;


namespace FCT.Control.Services
{
    public class DialogService : IDialogService
    {
        public UserResponse AskUser(string caption, string question)
        {
            var response = MessageBox.Show(question, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return response == DialogResult.Yes ? UserResponse.Affirmative : UserResponse.Negative;
        }

        public void ShowErrorMsg(string caption, string question)
        {
            MessageBox.Show(question, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfoMsg(string caption, string question)
        {
            MessageBox.Show(question, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
