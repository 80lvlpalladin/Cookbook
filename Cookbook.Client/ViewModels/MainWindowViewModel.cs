using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>
    /// TODO: summary
    /// </summary>
    public class MainWindowViewModel : PropertyChangedPropagator
    {
        private IEnumerable<RecipeViewModel> recipes;
        private bool isLoading;

        private async Task LoadRecipesAsync()
        {
            IsLoading = true;
            Recipes = await APIConsumer.GetRecipesAsync();
            foreach (var recipe in Recipes)
                recipe.LoadChildrenCommand.Execute(null);
            IsLoading = false;
        }

        private void ShowRecipeCreator(RecipeViewModel recipe = null)
        {
            RecipeEditor.EditorShown = true;
            RecipeEditor.Recipe = recipe;
        }

        private void ShowRecipeEditor(RecipeViewModel recipe)
        {
            ShowRecipeCreator(recipe);
            RecipeEditor.EditMode = true;
        }

        public RecipeEditorControlViewModel RecipeEditor { get; } = new RecipeEditorControlViewModel();

        public MainWindowViewModel()
        {
            LoadRecipesAsync();
            ShowRecipeCreatorCommand = new RelayCommand<RecipeViewModel>(ShowRecipeCreator);
            ShowRecipeEditorCommand = new RelayCommand<RecipeViewModel>(ShowRecipeEditor);

            RecipeEditor.RootLevelUpdated += (s, e) => LoadRecipesAsync();
        }

        public IEnumerable<RecipeViewModel> Recipes
        {
            get
            {
                return recipes;
            }
            set
            {
                recipes = value.OrderBy(recipe => recipe.Title);
                OnPropertyChanged();
            }
        }
        public ICommand ShowRecipeCreatorCommand { get; set; }
        public ICommand ShowRecipeEditorCommand { get; set; }


        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }
        
    }
}
