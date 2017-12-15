using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

namespace FCT.WindowControls.TableControl
{
    /// <summary>
    /// Interaction logic for CarDataControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public static readonly DependencyProperty TableItemsProperty =
        DependencyProperty.Register("TableItems", 
        typeof(ObservableCollection<object>), typeof(TableControl),
        new PropertyMetadata(null,onDepPropertyChanged));

        public ObservableCollection<object> TableItems
        {
            get { return (ObservableCollection<object>) GetValue(TableItemsProperty); }
            set { SetValue(TableItemsProperty, value); }
        }

        public static readonly DependencyProperty GroupByProperty =
        DependencyProperty.Register("GroupBy",typeof(string), typeof(TableControl),
            new PropertyMetadata(string.Empty));

        private static void onDepPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableControl = d as TableControl;
            tableControl?.InitializeControl();
        }

        public string GroupBy
        {
            get { return (string)GetValue(GroupByProperty); }
            set { SetValue(GroupByProperty, value); }
        }

        public ICollectionView TableItemsCollcetionView { get; private set; }
        public TableControlViewModel TableControlVM { get; private set; }

        public TableControl()
        {
            InitializeComponent();
        }

        private void OnCarItemsCollectionChange(object sender, EventArgs e)
        {
             InitializeControl();
        }

        private void InitializeControl()
        {

            TableItemsCollcetionView = CollectionViewSource.GetDefaultView(TableItems);
            if (TableItemsCollcetionView != null)
            {
                TableControlVM = new TableControlViewModel();
                TableControlVM.TableItems = TableItems;

                TableControlVM.GroupBySelection = GroupBy;
                TableControlVM.GroupDescriptions = TableItemsCollcetionView.GroupDescriptions;
                TableControlVM.GetTableGroupNames();

                TableControlVM.GroupingEnabled = !string.IsNullOrEmpty(GroupBy);
                if(string.IsNullOrEmpty(GroupBy) &&
                    TableControlVM.TableGroupNames.Count > 0)
                {
                    TableControlVM.GroupBySelection = TableControlVM.TableGroupNames[0];
                }
                if(TableControlVM.GroupingEnabled)
                {
                    TableControlVM.AddGrouping();
                }

                TableControlVM.RefreshAction = TableItemsCollcetionView.Refresh;

                UnregisterDataItemsCollectionEventHandlers();
                RegisterDataItemsCollectionEventHandlers();

                TableDataGrid.ItemsSource = TableItemsCollcetionView;
                DataContext = TableControlVM;
            }
        }

        public void UnregisterDataItemsCollectionEventHandlers()
        {
            TableItemsCollcetionView.CollectionChanged -= TableControlVM.OnCarDataCollectionChange;
            TableItemsCollcetionView.CurrentChanged -= TableControlVM.OnCarDataCellChange;
            TableItemsCollcetionView.Filter -= TableControlVM.OnCarDataFilterRequest;
            TableDataGrid.AutoGeneratingColumn -= TableControlVM.OnAutoGeneratingColumn;
        }

        public void RegisterDataItemsCollectionEventHandlers()
        {
            TableItemsCollcetionView.CollectionChanged += TableControlVM.OnCarDataCollectionChange;
            TableItemsCollcetionView.CurrentChanged += TableControlVM.OnCarDataCellChange;
            TableItemsCollcetionView.Filter += TableControlVM.OnCarDataFilterRequest;
            TableDataGrid.AutoGeneratingColumn += TableControlVM.OnAutoGeneratingColumn;
        }

    }
}
