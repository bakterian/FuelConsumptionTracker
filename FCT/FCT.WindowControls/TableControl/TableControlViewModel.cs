using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Reflection;
using FCT.Infrastructure.Attributes;

namespace FCT.WindowControls.TableControl
{
    public class TableControlViewModel
    {
        public ObservableCollection<object>  TableItems { get; set; }

        public ObservableCollection<GroupDescription> GroupDescriptions { get; set; }

        public string GroupBy { get; set; }

        public void AddGrouping()
        {
            if (!string.IsNullOrEmpty(GroupBy) && GroupDescriptions.Count.Equals(0))
            {
                GroupDescriptions.Add(new PropertyGroupDescription(GroupBy));
            }
        }

        public bool OnCarDataFilterRequest(object obj)
        {
            return true; //TODO Filter data
        }

        public void OnCarDataCellChange(object sender, EventArgs e)
        {
            //TODO fire event for car data cell change => update db entry?
        }

        public void OnCarDataCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO fire event and notify that element has been added. If not triggered by observable collection
        }

        internal void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if ((e.PropertyDescriptor as System.ComponentModel.PropertyDescriptor).
                 Attributes.OfType<PresentableItem>().Count() == 0) e.Cancel = true;
        }
    }
}
