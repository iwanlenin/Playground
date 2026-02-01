using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlayGroundApp.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels implementing the INotifyPropertyChanged interface.
    /// Provides common functionality for property change notification,
    /// enabling automatic UI updates when properties change in MVVM applications.
    /// All ViewModels should inherit from this class.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when a property value changes.
        /// The UI automatically subscribes to this event to refresh bindings.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method to set property values and raise PropertyChanged events.
        /// Compares the old and new values to avoid unnecessary notifications.
        /// Uses CallerMemberName to automatically capture the property name.
        /// </summary>
        /// <typeparam name="T">The type of the property being set.</typeparam>
        /// <param name="storage">Reference to the backing field for the property.</param>
        /// <param name="value">The new value to set.</param>
        /// <param name="propertyName">The name of the property (automatically provided by CallerMemberName).</param>
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            storage = value;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of a property change.
        /// Uses CallerMemberName to automatically capture the calling property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed (automatically provided by CallerMemberName).</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
