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
        new PropertyMetadata(null, OnTableItemsChanged));

        public static readonly DependencyProperty GroupByProperty =
        DependencyProperty.Register("GroupBy", typeof(string), typeof(TableControl),
            new PropertyMetadata(OnGroupByChange));

        private static readonly DependencyProperty FilterByProperty =
           DependencyProperty.Register("FilterBy", typeof(string), typeof(TableControl), new PropertyMetadata(null, OnFilterByChange));

        private static readonly DependencyProperty FilterPhraseProperty =
        DependencyProperty.Register("FilterPhrase", typeof(string), typeof(TableControl), new PropertyMetadata(null, OnFilterPhraseChange));

        public ObservableCollection<object> TableItems
        {
            get { return (ObservableCollection<object>) GetValue(TableItemsProperty); }
            set { SetValue(TableItemsProperty, value); }
        }

        private static void OnTableItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableControl = d as TableControl;
            tableControl?.InitializeControl();
        }

        public string GroupBy
        {
            get { return (string)GetValue(GroupByProperty); }
            set { SetValue(GroupByProperty, value); }
        }

        private static void OnGroupByChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableControl = d as TableControl;
            tableControl?.TableControlVM?.OnGroupByInternalChange(tableControl?.GroupBy);
        }

        public string FilterBy
        {
            get { return (string)GetValue(FilterByProperty); }
            set { SetValue(FilterByProperty, value); }
        }

        private static void OnFilterByChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableControl = d as TableControl;
            tableControl?.TableControlVM?.OnFilterByInternalChange(tableControl.FilterBy);
        }

        public string FilterPhrase
        {
            get { return (string)GetValue(FilterPhraseProperty); }
            set { SetValue(FilterPhraseProperty, value); }
        }

        private static void OnFilterPhraseChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableControl = d as TableControl;
            if (tableControl != null && tableControl.TableControlVM != null && tableControl.FilterPhrase != null)
            {
                tableControl.TableControlVM.FilterPhrase = tableControl.FilterPhrase;
            }
        }

        public ICollectionView TableItemsCollcetionView { get; private set; }
        public TableControlViewModel TableControlVM { get; private set; }

        public TableControl()
        {
            TableControlVM = new TableControlViewModel();
            InitializeComponent();
            LayoutRoot.DataContext = TableControlVM;
        }

        private void InitializeControl()
        {
            TableItemsCollcetionView = CollectionViewSource.GetDefaultView(TableItems);

            if (TableItemsCollcetionView != null)
            {
                TableControlVM.TableItems = TableItems;

                TableControlVM.GroupDescriptions = TableItemsCollcetionView.GroupDescriptions;
                TableControlVM.GetTableGroupNames();

                TableControlVM.RefreshAction = TableItemsCollcetionView.Refresh;

                TableControlVM.OnGroupByInternalChange(GroupBy); 
                TableControlVM.OnFilterByInternalChange(FilterBy);
                if(!string.IsNullOrEmpty(FilterPhrase)) TableControlVM.FilterPhrase = FilterPhrase;


                UnregisterDataItemsCollectionEventHandlers();
                RegisterDataItemsCollectionEventHandlers();

                TableDataGrid.ItemsSource = TableItemsCollcetionView;
                //DataContext = TableControlVM;
            }
        }

        public void UnregisterDataItemsCollectionEventHandlers()
        {
            TableItemsCollcetionView.Filter -= TableControlVM.OnCarDataFilterRequest;
            TableDataGrid.AutoGeneratingColumn -= TableControlVM.OnAutoGeneratingColumn;
            TableDataGrid.LoadingRow -= TableControlVM.OnLoadingRow;
            TableDataGrid.BeginningEdit -= TableControlVM.OnBeginningEdit;
            TableDataGrid.InitializingNewItem -= TableControlVM.OnInitializingNewItem;
        }

        public void RegisterDataItemsCollectionEventHandlers()
        {
            TableItemsCollcetionView.Filter += TableControlVM.OnCarDataFilterRequest;
            TableDataGrid.AutoGeneratingColumn += TableControlVM.OnAutoGeneratingColumn;
            TableDataGrid.LoadingRow += TableControlVM.OnLoadingRow;
            TableDataGrid.BeginningEdit += TableControlVM.OnBeginningEdit;
            TableDataGrid.InitializingNewItem += TableControlVM.OnInitializingNewItem;
        }
    }
}
