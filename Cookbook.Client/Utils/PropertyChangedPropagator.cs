using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cookbook.Client.Utils
{
    /// <summary>Helper class for propertyChanged event propagation</summary>
    public abstract class PropertyChangedPropagator : INotifyPropertyChanged
    {
        ///<inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        ///<inheritdoc/>
        public void OnPropertyChanged([CallerMemberName] string property = "") => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
