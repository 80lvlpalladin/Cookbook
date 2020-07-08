using Cookbook.Client.Models;
using Cookbook.Client.Models.DTOs;
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
        private bool _editorShown;
        private RecipeViewModel _recipe;

        private async Task SendRecipeAsync()
        {
            Task task;

            if (EditMode)
            {
                task = APIConsumer.UpdateRecipeAsync(new UpdateRecipeDto()
                {
                    Title = Title,
                    Description = Description,
                    RecipeID = Recipe.RecipeID
                });
            }
            else
            {
                task = APIConsumer.SendNewRecipeAsync(new NewRecipeDto()
                {
                    Title = Title,
                    Description = Description,
                    ParentAncestryPath = Recipe?.AncestryPath
                });
            }

            await task;

            if (task.IsCompletedSuccessfully && !EditMode)
            {
                if (Recipe is null)
                    RootLevelUpdated?.Invoke(null, null);
                else
                    Recipe.LoadChildrenCommand.Execute(null);                    
            }
            else if (task.IsCompletedSuccessfully && EditMode)
            {
                Recipe.Title = Title;
                Recipe.Description = Description;
                EditMode = false;
            }

            CloseEditorCommand.Execute(null);            
        }

        public event EventHandler RootLevelUpdated; 

        public RecipeEditorControlViewModel()
        {
            SendRecipeCommand = new RelayCommand(() => SendRecipeAsync());
            CloseEditorCommand = new RelayCommand(() => EditorShown = false);
        }

        public bool EditMode { get; set; }

        public RecipeViewModel Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                Title = _recipe?.Title;
                Description = _recipe?.Description;
            }
        }

        public bool EditorShown
        {
            get => _editorShown;
            set
            {
                if(!value)
                {
                    Recipe = null;
                    Title = null;
                    Description = null;
                }

                _editorShown = value;
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
        public ICommand CloseEditorCommand { get; set; }

    }
}
