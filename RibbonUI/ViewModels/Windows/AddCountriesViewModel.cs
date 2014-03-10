﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.Windows {
    class AddCountriesViewModel : INotifyPropertyChanged {
        private IEnumerable<ICountry> _countries;
        public event PropertyChangedEventHandler PropertyChanged;

        public AddCountriesViewModel() {
            AddCommand = new RelayCommand<Window>(w => {
                w.DialogResult = true;
                w.Close();
            });

            CancelCommand = new RelayCommand<Window>(w => {
                w.DialogResult = false;
                w.Close();
            });
        }

        public ICommand<Window> AddCommand { get; private set; }
        public ICommand<Window> CancelCommand { get; private set; }

        public IEnumerable<ICountry> Countries {
            get { return _countries; }
            set {
                if (Equals(value, _countries)) {
                    return;
                }
                _countries = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
