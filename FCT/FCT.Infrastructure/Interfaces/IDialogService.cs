using FCT.Infrastructure.Enums;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDialogService
    {
        UserResponse AskUser(string caption, string question);

        void ShowInfoMsg(string caption, string question);

        void ShowErrorMsg(string caption, string question);
    }
}
