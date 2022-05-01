using Client.Resources;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace MicroinvestProject.Utilities
{
    public class TranslationSource : INotifyPropertyChanged
    {
        public event Action LanguageChangeEvent;
        private static readonly TranslationSource instance = new TranslationSource();

        public static TranslationSource Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager resManager = Resources.ResourceManager;
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get 
            {
                return this.resManager.GetString(key, this.currentCulture); 
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return this.currentCulture; }
            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    var @event = this.PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                        OnLanguageChange();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnLanguageChange()
        {
            LanguageChangeEvent?.Invoke();
        }
    }
}
