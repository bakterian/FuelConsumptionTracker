using FCT.Control.Services;
using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Threading;
using System.Windows;
using Xunit;

namespace FCT.Control.UnitTests
{
    public class ThemeSwitcherUnitTests
    {
        private IList<ThemeMap> _themeMapping;
        public ThemeSwitcherUnitTests()
        {
            InitializeUriParser();
            InitTestData();
        }

        private void InitializeUriParser()
        {
            if (Application.Current == null) new Application();
            PackUriHelper.Create(new Uri("reliable://0"));
        }

        private void InitTestData()
        {
            _themeMapping = new List<ThemeMap>()
            {
                new ThemeMap()
                {
                    Theme = AppTheme.AeroDark,
                    GeneralSourceUri = new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml", UriKind.Relative),
                    ColorSourceUri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Metallic.xaml", UriKind.Relative)
                },
                new ThemeMap()
                {
                    Theme = AppTheme.AeroWhite,
                    GeneralSourceUri = new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml", UriKind.Relative),
                     ColorSourceUri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml", UriKind.Relative)
                },
                new ThemeMap()
                {
                    Theme = AppTheme.RoyalDark,
                    GeneralSourceUri = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative),
                    ColorSourceUri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Metallic.xaml", UriKind.Relative)
                },
                new ThemeMap()
                {
                    Theme = AppTheme.RoyalWhite,
                    GeneralSourceUri = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative),
                    ColorSourceUri = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml", UriKind.Relative)
                }
            };
        }

        [Fact]
        public void ShouldFindActiveTheme()
        {
            var expectedActiveTheme = AppTheme.RoyalWhite;

            var appDictionaries = new Collection<ResourceDictionary>()
            {
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative)
                },
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml", UriKind.Relative),
                }
            };
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml", UriKind.Relative)
            });

            var themeSwitcher = new ThemeSwitcher(_themeMapping, appDictionaries);

            var activeTheme = themeSwitcher.GetActiveTheme();

            Assert.True(activeTheme.HasValue);
            Assert.Equal(activeTheme, expectedActiveTheme);
        }

        [Fact]
        public void ShouldFindActiveThemeInComplexRersourceStructure()
        {
            var expectedActiveTheme = AppTheme.AeroDark;

            var appDictionaries = new Collection<ResourceDictionary>()
            {
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative)
                },
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml", UriKind.Relative),
                },
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative)
                },
            };
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml", UriKind.Relative)
            });
            appDictionaries[2].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[2].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Metallic.xaml", UriKind.Relative)
            });

            var themeSwitcher = new ThemeSwitcher(_themeMapping, appDictionaries);

            var activeTheme = themeSwitcher.GetActiveTheme();

            Assert.True(activeTheme.HasValue);
            Assert.Equal(activeTheme, expectedActiveTheme);
        }

        [Fact]
        public void ShouldNotFindActiveTheme()
        {
            var appDictionaries = new Collection<ResourceDictionary>()
            {
                new ResourceDictionary(),
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml", UriKind.Relative),
                }
            };
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });

            var themeSwitcher = new ThemeSwitcher(_themeMapping, appDictionaries);

            var activeTheme = themeSwitcher.GetActiveTheme();
            Assert.False(activeTheme.HasValue);
        }

        [Fact]
        public void ShouldSetTheme()
        {
            var appDictionaries = new Collection<ResourceDictionary>()
            {
                new ResourceDictionary()
                {
                    Source = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative)
                },
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml", UriKind.Relative),
                }
            };
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml", UriKind.Relative)
            });

            var themeSwitcher = new ThemeSwitcher(_themeMapping, appDictionaries);

            var result = themeSwitcher.SetTheme(AppTheme.RoyalDark);

            Assert.True(result);
            Assert.Equal(_themeMapping[2].GeneralSourceUri, appDictionaries[0].Source);
            Assert.Equal(_themeMapping[2].ColorSourceUri, appDictionaries[1].MergedDictionaries[2].Source);
        }

        [Fact]
        public void ShouldNotSetTheme()
        {
            var appDictionaries = new Collection<ResourceDictionary>()
            {
                new ResourceDictionary()
                {
                    Source = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", UriKind.Relative)
                },
                new ResourceDictionary
                {
                    Source = new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml", UriKind.Relative),
                }
            };
            appDictionaries[1].MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative),
            });

            Console.WriteLine();
            var themeSwitcher = new ThemeSwitcher(_themeMapping, appDictionaries);

            var result = themeSwitcher.SetTheme(AppTheme.RoyalDark);

            Assert.False(result);
        }

    }
}
