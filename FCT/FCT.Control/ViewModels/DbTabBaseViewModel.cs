﻿using Caliburn.Micro;
using FCT.Infrastructure.Attributes;
using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Events;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FCT.Control.ViewModels
{
    public abstract class DbTabBaseViewModel<T> : IDbTabViewModel, INotifyAppClosing, IHandle<CarSelectionChangedEvent>, INotifyDbActions, INotifyPropertyChanged where T : new()
    {
        public abstract string HeaderName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected List<Tuple<T, ItemAction>> TableDataCollectionActions;

        protected readonly IDialogService DialogService;

        protected readonly IDataTableMapper DataTableMapper;

        private string _filterBy;
        public string FilterBy
        {
            get { return _filterBy; }
            set
            {
                _filterBy = value;
                RaisePropertyChanged();
            }
        }

        private string _filterPhrase;
        public string FilterPhrase
        {
            get { return _filterPhrase; }
            set
            {
                _filterPhrase = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<object> _tableDataCollection;
        public ObservableCollection<object> TableDataCollection
        {
            get { return _tableDataCollection; }
            set
            {
                if (!value.Equals(_tableDataCollection))
                {
                    _tableDataCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DbTabBaseViewModel(
                IDialogService dialogService,
                IAppClosingNotifier appClosingNotifier,
                IDbActionsNotifier dbActionsNotifier,
                IDataTableMapper dataTableMapper,
                IEventAggregator eventAggregator
            )
        {    //TODO: dispose any listneres when exiting
            DialogService = dialogService;
            DataTableMapper = dataTableMapper;
            TableDataCollectionActions = new List<Tuple<T, ItemAction>>();
            eventAggregator.Subscribe(this);
        }

        protected abstract IEnumerable<T> GetTableEntries();

        protected abstract void PersistDisplayedData();

        public abstract void Handle(CarSelectionChangedEvent message);

        public virtual void Init()
        {
            var tableEntries = GetTableEntries();

            if (TableDataCollection != null)
            {
                UnregisterEventHandlers();
                TableDataCollection.Clear();

                foreach (var tableEntrie in tableEntries)
                {
                    TableDataCollection.Add(tableEntrie);
                }
            }
            else
            {
                TableDataCollection = new ObservableCollection<object>((IEnumerable<object>)tableEntries);
            }

            RegisterEventHandlers(tableEntries);
        }

        protected IEnumerable<T> GetTableEntriesByAction(ItemAction action)
        {
            return TableDataCollectionActions.Where(_ => _.Item2.Equals(action)).Select(_ => _.Item1);
        }

        protected void UnregisterEventHandlers()
        {
            TableDataCollection.CollectionChanged -= TableDataCollectionChanged;
        }

        protected void RegisterEventHandlers(IEnumerable<T> fuelConsumptionEntries)
        {
            TableDataCollection.CollectionChanged += TableDataCollectionChanged;
            foreach (var fuelConsEntry in fuelConsumptionEntries)
            {
                (fuelConsEntry as BaseDbModel).PropertyChanged += TableDataChanged;
            }
        }

        protected virtual void TableDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var addedItem = (T)e.NewItems[0];

                if ((addedItem as BaseDbModel).Id == 0)
                {
                    (addedItem as BaseDbModel).Id = TableDataCollection.Count;
                }

                (TableDataCollection.Last() as BaseDbModel).PropertyChanged += TableDataChanged;
                TableDataCollectionActions.Add(new Tuple<T, ItemAction>((T)e.NewItems[0], (ItemAction)e.Action));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedItem = (T)e.OldItems[0];

                TableDataCollectionActions.RemoveAll(_ => _.Item1.Equals(removedItem));

                TableDataCollectionActions.Add(new Tuple<T, ItemAction>(removedItem, (ItemAction)e.Action));
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var newAction = new Tuple<T, ItemAction>((T)e.NewItems[0], ItemAction.Update);

                var existingMatch = TableDataCollectionActions.FirstOrDefault(_ =>
                (_.Item1 as BaseDbModel).Id.Equals((newAction.Item1 as BaseDbModel).Id) && _.Item2.Equals(newAction.Item2));

                if (existingMatch != null)
                {
                    var index = TableDataCollectionActions.IndexOf(existingMatch);
                    TableDataCollectionActions[index] = newAction;
                }
                else
                {
                    TableDataCollectionActions.Add(newAction);
                }
            }
        }

        protected virtual void TableDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is T)
            {
                TableDataCollectionActions.Add(new Tuple<T, ItemAction>((T)sender, ItemAction.Update));
            }
        }

        public virtual void OnDbWrite()
        {
            if (TableDataCollectionActions.Count > 0) PersistDisplayedData();
        }

        public virtual void OnDbRead()
        {
            if (TableDataCollectionActions.Count > 0)
            {
                var res = DialogService.AskUser($"{HeaderName} Read",
                    $"{HeaderName} data was modified you will lose your data, do you want to continue with reading?");
                if (res == UserResponse.Negative) return;
            }

            Init();
        }

        public virtual void OnApplicationClose()
        {
            if (TableDataCollectionActions.Count > 0)
            {
                var res = DialogService.AskUser($"{HeaderName} Changed",
                    $"{HeaderName} was modified do you want to persist the changes?");
                if (res == UserResponse.Affirmative)
                {
                    PersistDisplayedData();
                }
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Task<DataTable> GetDataTableAsync()
        {
            return DataTableMapper.ConvertToDataTableAsync(TableDataCollection.Select(_ => (T)_), new[] { new PresentableItem() });
        }

        public virtual async Task UpdateDataAsync(IList<DataTable> dataTables)
        {
            var data = dataTables.FirstOrDefault(_ => _.TableName.Equals(HeaderName));

            var newTableCollectionItems = await DataTableMapper.ConvertToDbEnumerableAsync<T>(data, new[] { new PresentableItem() });

            var newCollection = newTableCollectionItems.ToArray();
            var collectionSize = TableDataCollection.Count;
            var newCollectionSize = newTableCollectionItems.Count();
            for (int i = 0; i < Math.Max(collectionSize, newCollectionSize); i++)
            {
                if (i >= collectionSize)
                {
                    (newCollection[i] as BaseDbModel).PropertyChanged += TableDataChanged;
                    TableDataCollection.Add(newCollection[i]);
                }
                else if (i >= newCollectionSize)
                {
                    TableDataCollection.RemoveAt(newCollectionSize);
                }
                else if (!TableDataCollection[i].Equals(newCollection[i]))
                {
                    (newCollection[i] as BaseDbModel).Id = (TableDataCollection[i] as BaseDbModel).Id;
                    (newCollection[i] as BaseDbModel).PropertyChanged += TableDataChanged;
                    TableDataCollection[i] = newCollection[i];
                }
            }
        }
    }
}
