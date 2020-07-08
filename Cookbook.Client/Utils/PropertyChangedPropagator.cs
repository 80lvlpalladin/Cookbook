using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cookbook.Client.Utils
{
    /// <summary>Helper class for propertyChanged event propagation</summary>
    public abstract class PropertyChangedPropagator : INotifyPropertyChanged
    {
        ///<inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Property changed event handler that is used for 'manual' raise of the event </summary>
        /// <param name="property">Name of the property raising the event</param>
        public void OnPropertyChanged([CallerMemberName] string property = "") => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>
        /// Overloaded version of <see cref="OnPropertyChanged(string)"/> for subscribing to PropertyChanged event
        ///</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => 
            PropertyChanged?.Invoke(sender, e);
    }
}
