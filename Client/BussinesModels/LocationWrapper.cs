using Client.Utilities;
using Database.Enums;
using MicroinvestProject.Utilities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Client.BussinesModels
{
    public class LocationWrapper : INotifyPropertyChanged
    {
        #region Declarations

        private string strLocation;

        #endregion

        #region Properties

        public Location Location { get; set; }

        public string StrLocation
        {
            get
            {
                return strLocation;
            }
            set
            {
                strLocation = value;
                NotifyPropertyChanged(nameof(StrLocation));
            }
        }

        #endregion

        #region Constructor

        public LocationWrapper(Location location)
        {
            Location = location;
            StrLocation = Resources.Resources.ResourceManager.GetString(EnumExtension.GetDescription(location),TranslationSource.Instance.CurrentCulture);
            TranslationSource.Instance.LanguageChangeEvent += LanguageHandler;
        }

        #endregion

        #region Methods
        public void LanguageHandler()
        {
            StrLocation = Resources.Resources.ResourceManager.GetString(EnumExtension.GetDescription(Location), TranslationSource.Instance.CurrentCulture);
        }

        public static List<LocationWrapper> LoadLocationList()
        {
            List<LocationWrapper> Locations = new List<LocationWrapper>();

            Locations.Add(new LocationWrapper(Location.Sofia));
            Locations.Add(new LocationWrapper(Location.Plovdiv));
            Locations.Add(new LocationWrapper(Location.Varna));
            Locations.Add(new LocationWrapper(Location.Burgas));
            Locations.Add(new LocationWrapper(Location.Ruse));

            return Locations;
        }

        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
