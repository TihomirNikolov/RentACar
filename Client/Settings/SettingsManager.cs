using Client.Helpers;

namespace Client.Settings
{
    public static class SettingsManager
    {
        #region Declarations

        private static string dbServer;
        private static string dbUsername;
        private static string dbPassword;
        private static string dbName;
        private static string language;

        private static XMLFileHelper xml;

        #endregion

        static SettingsManager()
        {
            xml = new XMLFileHelper();

            xml.CreateXMLIfNotExists();
        }

        #region Properties

        public static string DbServer
        {
            get
            {
                if (dbServer == null)
                {
                    dbServer = CryptHelper.Instance.Decrypt(xml.ReadSettingsValue());
                }
                return dbServer;
            }

            set
            {
                if (dbServer != value)
                {
                    xml.UpdateSettingsValue(CryptHelper.Instance.Encrypt(value));
                    dbServer = value;
                }
            }
        }

        public static string DbUsername
        {
            get
            {
                if (dbUsername == null)
                {
                    dbUsername = CryptHelper.Instance.Decrypt(xml.ReadSettingsValue());
                }
                return dbUsername;
            }

            set
            {
                if (dbUsername != value)
                {
                    xml.UpdateSettingsValue(CryptHelper.Instance.Encrypt(value));
                    dbUsername = value;
                }
            }
        }

        public static string DbPassword
        {
            get
            {
                if (dbPassword == null)
                {
                    dbPassword = xml.ReadSettingsValue();
                }
                return dbPassword;
            }

            set
            {
                if (dbPassword != value)
                {
                    xml.UpdateSettingsValue(value);
                    dbPassword = value;
                }
            }
        }

        public static string DbName
        {
            get
            {
                if (dbName == null)
                {
                    dbName = CryptHelper.Instance.Decrypt(xml.ReadSettingsValue());
                }
                return dbName;
            }

            set
            {
                if (dbName != value)
                {
                    xml.UpdateSettingsValue(CryptHelper.Instance.Encrypt(value));
                    dbName = value;
                }
            }
        }

        public static string Language
        {
            get
            {
                if(language == null)
                {
                    language = xml.ReadSettingsValue();
                }
                return language;
            }

            set
            {
                if (language != value)
                {
                    xml.UpdateSettingsValue(value);
                    language = value;
                }
            }
        }

        #endregion


    }
}
