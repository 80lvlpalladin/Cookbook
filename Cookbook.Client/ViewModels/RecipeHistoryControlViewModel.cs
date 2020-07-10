using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>ViewModel for RecipeHistoryControl</summary>
    public class RecipeHistoryControlViewModel : PropertyChangedPropagator
    {
        private bool _historyShown = false;
        private IEnumerable<RecipeLogEntry> _logEntries;

        /// <summary>Indicates whether RecipeHistoryControl should be shown to user</summary>
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

        /// <summary>Recipe log entries shown to user</summary>
        public IEnumerable<RecipeLogEntry> LogEntries
        {
            get => _logEntries;
            set
            {
                _logEntries = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Command that is invoked on Cancel button click</summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>Constructor</summary>
        public RecipeHistoryControlViewModel() => CloseCommand = new RelayCommand(() => HistoryShown = false);
    }
}
