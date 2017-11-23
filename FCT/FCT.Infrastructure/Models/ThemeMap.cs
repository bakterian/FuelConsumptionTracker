using FCT.Infrastructure.Enums;
using System;

namespace FCT.Infrastructure.Models
{
    public class ThemeMap
    {
        public AppTheme Theme { get; set; }
        public Uri GeneralSourceUri { get; set; }
        public Uri ColorSourceUri { get; set; }
    }
}
