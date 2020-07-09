using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    public class RecipeHistoryControlViewModel : PropertyChangedPropagator
    {
        private bool _historyShown = false;
        private IEnumerable<RecipeLogEntry> _logEntries;

        public bool HistoryShown
        {
            get => _historyShown;
            set
            {
                if (!value)
                    LogEntries = null;
                _historyShown = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<RecipeLogEntry> LogEntries
        {
            get => _logEntries;
            set
            {
                _logEntries = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; set; }

        public RecipeHistoryControlViewModel()
        {
            CloseCommand = new RelayCommand(() => HistoryShown = false);
        }
    }
}
