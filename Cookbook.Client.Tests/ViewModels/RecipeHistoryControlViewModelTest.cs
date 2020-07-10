using Cookbook.Client.Models;
using Cookbook.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cookbook.Client.Tests.ViewModels
{
    /// <summary>Tests for RecipeHistoryControl</summary>
    public class RecipeHistoryControlViewModelTest
    {
        [Fact]
        public void LogEntriesAreSetToNull_IfHistoryShownIsFalse()
        {
            //Arrange
            var historyVM = new RecipeHistoryControlViewModel();
            var logEntries = new[] { new RecipeLogEntry() };
            historyVM.LogEntries = logEntries;
            historyVM.HistoryShown = true;

            //Act
            historyVM.HistoryShown = false;

            //Assert
            Assert.Null(historyVM.LogEntries);
        }
    }
}
