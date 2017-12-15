using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Reflection;
using FCT.Infrastructure.Attributes;
using System.Runtime.CompilerServices;

namespace FCT.WindowControls.TableControl
{
    public class TableControlViewModel : INotifyPropertyChanged
    {
        private bool _groupingEnabled = false;
        private bool _filteringEnabled = false;
        private string _groupBySelection = string.Empty;
        private string _filterBySelection = string.Empty;
        private string _filterPhrase = string.Empty;

        public Action RefreshAction { get; set; }

        public ObservableCollection<object>  TableItems { get; set; }

        public ObservableCollection<GroupDescription> GroupDescriptions { get; set; }

        public ObservableCollection<string> TableGroupNames { get; set; } = new ObservableCollection<string>();


        public string GroupBySelection
        {
            get { return _groupBySelection; }
            set
            {
                if(!string.IsNullOrEmpty(value) && !value.Equals(_groupBySelection))
                {
                    _groupBySelection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FilterBySelection
        {
            get { return _filterBySelection; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals(_filterBySelection))
                {
                    _filterBySelection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool GroupingEnabled
        {
            get { return _groupingEnabled; }
            set
            {
                if (!value.Equals(_groupingEnabled))
                {
                    _groupingEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool FilteringEnabled
        {
            get { return _filteringEnabled; }
            set
            {
                if (!value.Equals(_filteringEnabled))
                {
                    _filteringEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FilterPhrase
        {
            get { return _filterPhrase; }
            set
            {
                if (!value.Equals(_filterPhrase))
                {
                    _filterPhrase = value;
                    RaisePropertyChanged();
                    OnFilterByChange();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddGrouping()
        {
            if (!string.IsNullOrEmpty(GroupBySelection) && GroupDescriptions.Count.Equals(0))
            {
                GroupDescriptions.Add(new PropertyGroupDescription(GroupBySelection));
            }
        }

        public void GetTableGroupNames()
        {
            if(TableItems != null && TableItems.Count > 0)
            {
                foreach (var itemProperty in TableItems[0].GetType().GetRuntimeProperties())
                {
                    if(itemProperty.CustomAttributes.
                        Count(_ => _.AttributeType.Equals(typeof(PresentableItem))) > 0)
                    {
                        TableGroupNames.Add(itemProperty.Name);
                    }
                }
            }
        }

        public void OnGroupBySelectionChange()
        {
            GroupDescriptions.Clear();
            AddGrouping();
        }

        public void OnGroupByEnableChange()
        {
            if (GroupingEnabled)
            {
                AddGrouping();
            }
            else
            {
                GroupDescriptions.Clear();
            }
        }

        public void OnFilterByChange()
        {
            RefreshAction?.Invoke();
        }

        internal void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if ((e.PropertyDescriptor as System.ComponentModel.PropertyDescriptor).
                 Attributes.OfType<PresentableItem>().Count() == 0) e.Cancel = true;
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool OnCarDataFilterRequest(object obj)
        {
            var isMatching = true;
            if(FilteringEnabled && !string.IsNullOrEmpty(FilterPhrase))
            {
                isMatching = obj.GetType()?.
                    GetRuntimeProperty(FilterBySelection)?.GetValue(obj)?.
                    ToString().Contains(FilterPhrase) ?? false;

            }
            return isMatching;
        }

        public void OnCarDataCellChange(object sender, EventArgs e)
        {
            //TODO fire event for car data cell change => update db entry?
        }

        public void OnCarDataCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO fire event and notify that element has been added. If not triggered by observable collection
        }
    }
}
