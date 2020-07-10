using Cookbook.Client.Models;
using Cookbook.Client.Models.DTOs;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>ViewModel for RecipeEditorControl</summary>
    public class RecipeEditorControlViewModel : PropertyChangedPropagator
    {
        private string _title;
        private string _description;
        private bool _editorShown;
        private RecipeViewModel _recipe;
        private bool _submitEnabled = false;

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

        private bool Validate()
        {
            if (IsNullEmpty(_title) || IsNullEmpty(_description)) //checks if either of fields is not empty
                return false;

            if (_recipe == null)
            {
                if (RootLevelTitles.Any(title => title == _title))
                    return false;
                else
                    return true;
            }
                
            if (_recipe.Title == _title && _recipe.Description == _description)
                return false;

            if (!EditMode)
            {
                if (_recipe.Children != null)
                    return !_recipe.Children.Any(recipe => recipe.Title == _title && recipe.Description == _description);
            }
            return true;           
        }

        private bool IsNullEmpty(string text)
        {
            return (text is null || text.Length == 0);
        }

        /// <summary>Event indicating that root level of the recipe tree should be updated</summary>
        public event EventHandler RootLevelUpdated;

        /// <summary>Needed for validating new root level recipe names </summary>
        public IEnumerable<string> RootLevelTitles { get; set; }

        /// <summary>Constructor. Sets commands. </summary>
        public RecipeEditorControlViewModel()
        {
            SendRecipeCommand = new RelayCommand(() => SendRecipeAsync());
            CloseEditorCommand = new RelayCommand(() => EditorShown = false);
        }

        /// <summary>Indicates whether editor should send new recipe or update existing one</summary>
        public bool EditMode { get; set; }

        /// <summary>Recipe that is being edited</summary>
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

        /// <summary>Indicates whether editor should be shown to user or not</summary>
        public bool EditorShown
        {
            get => _editorShown;
            set
            {
                if (!value)
                {
                    Recipe = null;
                    Title = null;
                    Description = null;
                }

                _editorShown = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Indicates whether submit button is enabled or not</summary>
        public bool SubmitEnabled
        {
            get => _submitEnabled;
            set
            {
                _submitEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Title in Editor TextBox</summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SubmitEnabled = Validate();
                OnPropertyChanged();
            }
        }

        /// <summary>Description in Editor TextBox</summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                SubmitEnabled = Validate();
                OnPropertyChanged();
            }
        }

        /// <summary>Command invoked on click of Submit button</summary>
        public ICommand SendRecipeCommand { get; set; }

        /// <summary>Command invoked on click of Cancel button</summary>
        public ICommand CloseEditorCommand { get; set; }

    }
}
