using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>ViewModel for MainWindow</summary>
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

        private async Task LoadRecipeHistoryAsync(int recipeId)
        {
            RecipeHistory.LogEntries = await APIConsumer.GetHistoryAsync(recipeId);
            RecipeHistory.HistoryShown = true;
        }

        private void ShowRecipeCreator(RecipeViewModel recipe = null)
        {
            RecipeEditor.EditorShown = true;
            RecipeEditor.Recipe = recipe;

            if (recipe == null)
                RecipeEditor.RootLevelTitles = Recipes.Select(recipe => recipe.Title);
        }

        private void ShowRecipeEditor(RecipeViewModel recipe)
        {
            ShowRecipeCreator(recipe);
            RecipeEditor.EditMode = true;
        }

        /// <summary>RecipeEditor instance</summary>
        public RecipeEditorControlViewModel RecipeEditor { get; } = new RecipeEditorControlViewModel();

        /// <summary>RecipeHistory instance</summary>
        public RecipeHistoryControlViewModel RecipeHistory { get; } = new RecipeHistoryControlViewModel();
      
        /// <summary>Does initial recipe loading, sets up commands and subscribes to events </summary>
        public MainWindowViewModel()
        {
            LoadRecipesAsync();
            ShowRecipeCreatorCommand = new RelayCommand<RecipeViewModel>(ShowRecipeCreator);
            ShowRecipeEditorCommand = new RelayCommand<RecipeViewModel>(ShowRecipeEditor);
            ShowRecipeHistoryCommand = new RelayCommand<int>(id => LoadRecipeHistoryAsync(id));
            RecipeEditor.RootLevelUpdated += (s, e) => LoadRecipesAsync();
        }

        /// <summary>Root level recipes collection</summary>
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

        /// <summary>Command invoked when Create Recipe or Fork Recipe button is clicked</summary>
        public ICommand ShowRecipeCreatorCommand { get; set; }

        /// <summary>Command invoked when Edit Recipe button is clicked</summary>
        public ICommand ShowRecipeEditorCommand { get; set; }

        /// <summary>Command invoked when Recipe History button is clicked</summary>
        public ICommand ShowRecipeHistoryCommand { get; set; }

        /// <summary>Indicates wether Root level recipes are being loaded </summary>
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
