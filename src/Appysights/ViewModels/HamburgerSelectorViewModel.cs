﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Appysights.Models;
using Appysights.Services;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace Appysights.ViewModels
{
    public class HamburgerSelectorViewModel : PropertyChangedBase
    {
        private ConfigurationManager _manager;
        private Action<IMenuItem> _onClick;
        private bool _isOpen;

        public HamburgerSelectorViewModel(IEnumerable<IMenuItem> items, Action<IMenuItem> onClick, ConfigurationManager manager)
        {
            _onClick = onClick;
            Items = new ObservableCollection<IMenuItem>(items);
            //OptionItems = new ObservableCollection<IMenuItem>(new List<IMenuItem>() { new MenuItem("Option") });
            SelectedItem = Items.FirstOrDefault();
            manager.NewConfiguration += Manager_NewConfiguration;
        }

        private void Manager_NewConfiguration(object sender, ConfigurationService e)
        {
            Items.Add(new DashboardViewModel(e.Entity));
            NotifyOfPropertyChange(() => Items);
        }

        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }

            set
            {
                _isOpen = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<IMenuItem> Items { get; set; }

        public ObservableCollection<IMenuItem> OptionItems { get; set; }

        public IMenuItem SelectedItem { get; set; }

        //public bool Multiple => Items.Count > 1;
        public bool Multiple => true;

        public bool Single => !Multiple;

        public void MenuSelectionChanged(object value, ItemClickEventArgs args)
        {
            var dashboard = args.ClickedItem as DashboardViewModel;
            if (dashboard != null)
            {
                _onClick?.Invoke(dashboard);
                IsOpen = false;
            }
        }
    }
}
