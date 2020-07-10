using Cookbook.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cookbook.Client.Tests.ViewModels
{
    /// <summary>
    /// Tests for RecipeEditorControlViewModel
    /// </summary>
    public class RecipeEditorControlViewModelTest
    {
        [Fact]
        public void SubmitEnabled_ReturnsFalse_IfChildRecipeTitleAndEditorTitleMatch()
        {
            //Arrange
            var editorVM = new RecipeEditorControlViewModel();
            var recipeVM = new RecipeViewModel();
            recipeVM.Title = "Recipe Title";
            var childRecipeVM = new RecipeViewModel();
            childRecipeVM.Title = "Child Recipe Title";
            recipeVM.Children = new[] { childRecipeVM };
            editorVM.Recipe = recipeVM;

            //Act
            editorVM.Title = "Child Recipe Title";

            //Assert
            Assert.False(editorVM.SubmitEnabled);
        }

        [Fact]
        public void SubmitEnabled_ReturnsFalse_IfRecipeTitleAndEditorTitleMatch()
        {
            //Arrange
            var editorVM = new RecipeEditorControlViewModel();
            var recipeVM = new RecipeViewModel();
            recipeVM.Title = "Recipe Title";
            var childRecipeVM = new RecipeViewModel();
            childRecipeVM.Title = "Child Recipe Title";
            recipeVM.Children = new[] { childRecipeVM };
            editorVM.Recipe = recipeVM;

            //Act
            editorVM.Title = "Recipe Title";

            //Assert
            Assert.False(editorVM.SubmitEnabled);
        }

        [Fact]
        public void SubmitEnabled_InEditMode_ReturnsFalse_IfChildRecipeTitleAndEditorTitleMatch()
        {
            //Arrange
            var editorVM = new RecipeEditorControlViewModel();
            var recipeVM = new RecipeViewModel();
            recipeVM.Title = "Recipe Title";
            var childRecipeVM = new RecipeViewModel();
            childRecipeVM.Title = "Child Recipe Title";
            recipeVM.Children = new[] { childRecipeVM };
            editorVM.Recipe = recipeVM;
            editorVM.EditMode = true;

            //Act
            editorVM.Title = "Child Recipe Title";

            //Assert
            Assert.False(editorVM.SubmitEnabled);
        }

        [Fact]
        public void SubmitEnabled_InEditMode_ReturnsFalse_IfRecipeTitleAndEditorTitleMatch()
        {
            //Arrange
            var editorVM = new RecipeEditorControlViewModel();
            var recipeVM = new RecipeViewModel();
            recipeVM.Title = "Recipe Title";
            var childRecipeVM = new RecipeViewModel();
            childRecipeVM.Title = "Child Recipe Title";
            recipeVM.Children = new[] { childRecipeVM };
            editorVM.Recipe = recipeVM;
            editorVM.EditMode = true;

            //Act
            editorVM.Title = "Recipe Title";

            //Assert
            Assert.False(editorVM.SubmitEnabled);
        }

        [Fact]
        public void RecipeTitleDescription_AreNull_IfEditorShownIsFalse()
        {
            //Arrange
            var title = "Recipe Title";
            var descr = "Recipe Description";
            var editorVM = new RecipeEditorControlViewModel();
            var recipeVM = new RecipeViewModel() { Title = title, Description = descr };
            editorVM.Recipe = recipeVM;

            //additional check
            Assert.Equal(editorVM.Title, recipeVM.Title);
            Assert.Equal(editorVM.Description, recipeVM.Description);
            Assert.Equal(editorVM.Recipe, recipeVM);

            //Act
            editorVM.EditorShown = false;

            //Assert
            Assert.Null(editorVM.Title);
            Assert.Null(editorVM.Description);
            Assert.Null(editorVM.Recipe);
        }
       
    }
}
