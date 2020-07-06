using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>
    /// TODO: summary
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
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

        private async Task<bool> CreateRecipeAsync()
        {
            return true;
        }

        public MainWindowViewModel()
        {
            LoadRecipesAsync();
            CreateRecipeCommand = new AsyncCommand(CreateRecipeAsync);
        }

        public ICommand CreateRecipeCommand { get; set; }

        public RecipeViewModel SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                OnPropertyChanged();
            }
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

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
