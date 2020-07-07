using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Client.ViewModels
{
    /// <summary>
    /// TODO: summary
    /// </summary>
    public class MainWindowViewModel : PropertyChangedPropagator
    {
        private IEnumerable<RecipeViewModel> recipes;
        private bool isLoading;
        private RecipeViewModel selectedRecipe;

        private async Task LoadRecipesAsync()
        {
            IsLoading = true;
            Recipes = await APIConsumer.GetRecipesAsync();
            foreach (var recipe in Recipes)
                recipe.LoadChildrenCommand.Execute(null);
            IsLoading = false;
        }

        public RecipeEditorControlViewModel RecipeEditor { get; } = new RecipeEditorControlViewModel();

        public MainWindowViewModel() => LoadRecipesAsync();

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
