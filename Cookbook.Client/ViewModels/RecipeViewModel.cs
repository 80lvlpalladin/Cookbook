using Cookbook.Client.Models;
using Cookbook.Client.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.ViewModels
{
    /// <summary>Client-side recipe class</summary>
    public class RecipeViewModel : INotifyPropertyChanged
    {
        private IEnumerable<RecipeViewModel> children;

        private async Task LoadChildren()
        {
            Children = await APIConsumer.GetRecipesAsync(RecipeID);
        }

        public RecipeViewModel()
        {
            LoadChildrenCommand = new AsyncCommand(LoadChildren);
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


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
