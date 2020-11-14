using Caliburn.Micro;
using SpatialCheck.Localization;

namespace SpatialCheck
{
    public class TranslationBindingProvider : PropertyChangedBase
    {
        public static TranslationBindingProvider Instance { get; } = new TranslationBindingProvider();

        private TranslationBindingProvider()
        {
            TranslationManager.CurrentLanguageChangedEvent += (sender, args) => NotifyOfPropertyChange(string.Empty);
        }

        public string this[string key] => Strings.ResourceManager.GetString(key);
    }
}
