using FCT.Infrastructure.Enums;

namespace FCT.Infrastructure.Interfaces
{
    public interface IThemeSwitcher
    {
        AppTheme? GetActiveTheme();

        bool SetTheme(AppTheme theme);
    }
}
