using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>View model class for recipe</summary>
    public class RecipeViewModel : PropertyChangedPropagator
    {
        private IEnumerable<RecipeViewModel> children;

        private async Task LoadChildrenAsync()
        {
            Children = await APIConsumer.GetRecipesAsync(RecipeID);
        }

        public RecipeViewModel()
        {
            LoadChildrenCommand = new RelayCommand(() => LoadChildrenAsync());
        }

        /// <summary>Unique identifier of the recipe</summary>
        public int RecipeID { get; set; }

        /// <summary>
        /// Unique identifier of the position of the recipe in recipe tree.
        /// By default, if no ancestry type is specified, Recipe is placed on the top level of the tree
        /// </summary>
        public string AncestryPath { get; set; }

        /// <summary>Date recipe was last updated</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>Recipe title</summary>
        public string Title { get; set; }

        /// <summary>Recipe description</summary>
        public string Description { get; set; }

        public ICommand LoadChildrenCommand { get; set; }

        public IEnumerable<RecipeViewModel> Children
        {
            get => children;
            set
            {
                children = value.OrderBy(recipe => recipe.Title);
                OnPropertyChanged();
            }
        }
    }
}
