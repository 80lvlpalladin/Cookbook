using Cookbook.Client.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Cookbook.Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Recipe> recipes;

        private async Task LoadRecipes()
        {
            using var consumer = new APIConsumer();
            Recipes = await consumer.GetRecipesAsync();          
        }

        public MainWindowViewModel()
        {
            LoadRecipes();
        }

        public IEnumerable<Recipe> Recipes
        {
            get 
            { 
                return recipes;
            }
            set
            {
                recipes = value;
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
