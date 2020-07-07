using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    public class RecipeEditorControlViewModel : PropertyChangedPropagator
    {
        private string _title;
        private string _description;
        private string ancestryPath;
        private bool _editorShown;





        private async Task SendRecipeAsync()
        {
            await APIConsumer.SendRecipeAsync(Title, Description, AncestryPath);
        }

        public RecipeEditorControlViewModel()
        {
            SendRecipeCommand = new RelayCommand(() => SendRecipeAsync());
        }

        public bool EditorShown
        {
            get => _editorShown;
            set
            {
                _editorShown = value;
                OnPropertyChanged();
            }
        }

        public string AncestryPath
        {
            get => ancestryPath;
            set
            {
                ancestryPath = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendRecipeCommand { get; set; }
    }
}
