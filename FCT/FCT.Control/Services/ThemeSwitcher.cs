using FCT.Infrastructure.Interfaces;
using System;
using FCT.Infrastructure.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using FCT.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace FCT.Control.Services
{
    public class ThemeSwitcher : IThemeSwitcher
    {
        private readonly Collection<ResourceDictionary> _resourceDictionaries;
        private readonly IList<ThemeMap> _themeMapping;

        public ThemeSwitcher(IList<ThemeMap> themeMapping,
                             Collection<ResourceDictionary> resourceDictionaries)
        {
            _resourceDictionaries = resourceDictionaries;
            _themeMapping = themeMapping;
        }

        public AppTheme? GetActiveTheme()
        {
            AppTheme? activeTheme = null;

            foreach (var map in _themeMapping)
            {
                var foundDictionary = GetDictionaryContaingSource(_resourceDictionaries, map.GeneralSourceUri);
                if (foundDictionary == null) continue;
                foundDictionary = GetDictionaryContaingSource(_resourceDictionaries, map.ColorSourceUri);
                if(foundDictionary != null)
                {
                    activeTheme = map.Theme;
                    break;
                }
            }

            return activeTheme;
        }

        public bool SetTheme(AppTheme theme)
        {
            var switchSuccessfull = false;
            var newThemeMap = _themeMapping.FirstOrDefault(_ => _.Theme.Equals(theme));
            if (newThemeMap != null)
            {
                ResourceDictionary colorThemeDictionary = null;
                var generalThemeDictionary = FindSupportedDictionary(_resourceDictionaries, _themeMapping.Select(_ => _.GeneralSourceUri));
                if(generalThemeDictionary != null)
                {
                    colorThemeDictionary = FindSupportedDictionary(_resourceDictionaries, _themeMapping.Select(_ => _.ColorSourceUri));
                    if(colorThemeDictionary != null)
                    {
                        UpdateRescources(generalThemeDictionary, newThemeMap.GeneralSourceUri);
                        UpdateRescources(colorThemeDictionary, newThemeMap.ColorSourceUri);
                        switchSuccessfull = true;
                    }
                }
            }
            return switchSuccessfull;
        }

        private void UpdateRescources(ResourceDictionary oldThemeDictionary, Uri NewThemeUri)
        {
            try
            {
                if (IsTopLevelDictionary(_resourceDictionaries, oldThemeDictionary))
                {
                    var id = _resourceDictionaries.IndexOf(oldThemeDictionary);
                    _resourceDictionaries[id] = Application.LoadComponent(NewThemeUri) as ResourceDictionary;
                    _resourceDictionaries[id].Source = NewThemeUri;
                }
                else
                {
                    var parentAndIndex = GetParentofChild(_resourceDictionaries, oldThemeDictionary);
                    if (parentAndIndex != null)
                    {
                        var parent = parentAndIndex.Item1;
                        var childIndex = parentAndIndex.Item2;

                        parent.MergedDictionaries[childIndex] = Application.LoadComponent(NewThemeUri) as ResourceDictionary;
                        parent.MergedDictionaries[childIndex].Source = NewThemeUri;

                        if (IsTopLevelDictionary(_resourceDictionaries, parent))
                        {
                            var id = _resourceDictionaries.IndexOf(parent);
                            _resourceDictionaries[id] = parent;
                        }
                        else
                        {
                            var grandParentAndIndex = GetParentofChild(_resourceDictionaries, parent);
                            if (grandParentAndIndex != null)
                            {
                                var grandParent = grandParentAndIndex.Item1;
                                var parentIndex = grandParentAndIndex.Item2;
                                grandParent.MergedDictionaries[parentIndex] = parent;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception during loading resources {e.Message}");
                throw;
            }
        }

        private ResourceDictionary FindSupportedDictionary(Collection<ResourceDictionary> resourceDictionaries, IEnumerable<Uri> supportedUris)
        {
            ResourceDictionary themeDictionary = null;
            foreach (var supportedUri in supportedUris)
            {
                themeDictionary = GetDictionaryContaingSource(resourceDictionaries, supportedUri);
                if (themeDictionary != null) break;
            }
            return themeDictionary;
        }

        private ResourceDictionary GetDictionaryContaingSource(Collection<ResourceDictionary> resourceDictionaries, Uri searchedUriSource)
        {
            ResourceDictionary foundDictionary = null;
            if (resourceDictionaries?.Count > 0 &&  searchedUriSource != null)
            {
                foreach (var dictionary in resourceDictionaries)
                {
                    foundDictionary = (dictionary.Source != null && dictionary.Source.Equals(searchedUriSource)) ? dictionary :
                                       GetDictionaryContaingSource(dictionary.MergedDictionaries, searchedUriSource);

                    if (foundDictionary != null ) break;
                }
            }
            return foundDictionary;
        }


        private Tuple<ResourceDictionary, int> GetParentofChild(Collection<ResourceDictionary> resourceDictionaries, ResourceDictionary child)
        {
            Tuple<ResourceDictionary, int> foundDictionary = null;

            if (resourceDictionaries?.Count > 0 && child != null)
            {
                foreach (var dictionary in resourceDictionaries)
                {
                    if (dictionary.Equals(child))break;

                    foundDictionary = GetParentRecursive(dictionary.MergedDictionaries, child);               

                    if (foundDictionary != null)
                    {
                        if (foundDictionary.Item2 == -1)
                        {// in case first recursive function succeeds
                            var index = dictionary.MergedDictionaries.IndexOf(foundDictionary.Item1);
                            foundDictionary = new Tuple<ResourceDictionary, int>(dictionary, index);
                        }
                        break;
                    }
                }
            }
            return foundDictionary;
        }


        private Tuple<ResourceDictionary,int> GetParentRecursive(Collection<ResourceDictionary> resourceDictionaries, ResourceDictionary child)
        {
            Tuple<ResourceDictionary, int> foundDictionary = null;

            if (resourceDictionaries?.Count > 0 && child != null)
            {
                foreach (var dictionary in resourceDictionaries)
                {
                    if (dictionary.Equals(child))
                    {//if -1 means found child already, calling function to determin parent
                        foundDictionary = new Tuple<ResourceDictionary, int>(dictionary,-1);
                        break;
                    }
                    else
                    {
                        foundDictionary = GetParentRecursive(dictionary.MergedDictionaries, child);

                        if(foundDictionary != null)
                        {
                            if(foundDictionary.Item2 == -1)
                            {
                                var index = dictionary.MergedDictionaries.IndexOf(foundDictionary.Item1);
                                foundDictionary = new Tuple<ResourceDictionary, int>(dictionary, index);
                            }
                            break;
                        }
                    }       
                }
            }
            return foundDictionary;
        }

        private bool IsTopLevelDictionary(Collection<ResourceDictionary> resourceDictionaries, ResourceDictionary dictionary)
        {
            return resourceDictionaries.Contains(dictionary);
        }
    }
}
