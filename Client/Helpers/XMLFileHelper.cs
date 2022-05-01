using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Client.Helpers
{
    public class XMLFileHelper
    {
        #region Declarations

        private static string XMLFileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RentACar";
        private static string XMLFileName = "//Settings.xml";
        private static string XMLFilePath = XMLFileDirectory + XMLFileName;
        
        #endregion

        #region Methods

        public void CreateXMLIfNotExists()
        {
            if (!File.Exists(XMLFilePath))
            {
                if (!Directory.Exists(XMLFileDirectory))
                {
                    Directory.CreateDirectory(XMLFileDirectory);
                }

                CreateXMLSettingsFile();
            }
        }

        private void CreateXMLSettingsFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode settingsNode = doc.CreateElement("Settings");
                doc.AppendChild(settingsNode);

                XmlNode dbServer = doc.CreateElement(string.Empty, "DbServer", string.Empty);
                XmlAttribute attribute1 = doc.CreateAttribute("value");
                attribute1.Value = "";
                dbServer.Attributes.Append(attribute1);
                settingsNode.AppendChild(dbServer);

                XmlNode dbUsername = doc.CreateElement(string.Empty, "DbUsername", string.Empty);
                XmlAttribute attribute2 = doc.CreateAttribute("value");
                attribute2.Value = "";
                dbUsername.Attributes.Append(attribute2);
                settingsNode.AppendChild(dbUsername);

                XmlNode dbPassword = doc.CreateElement(string.Empty, "DbPassword", string.Empty);
                XmlAttribute attribute3 = doc.CreateAttribute("value");
                attribute3.Value = "";
                dbPassword.Attributes.Append(attribute3);
                settingsNode.AppendChild(dbPassword);

                XmlNode dbName = doc.CreateElement(string.Empty, "DbName", string.Empty);
                XmlAttribute attribute4 = doc.CreateAttribute("value");
                attribute4.Value = "";
                dbName.Attributes.Append(attribute4);
                settingsNode.AppendChild(dbName);

                XmlNode lang = doc.CreateElement(string.Empty, "Language", string.Empty);
                XmlAttribute attribute5 = doc.CreateAttribute("value");
                attribute5.Value = "";
                lang.Attributes.Append(attribute5);
                settingsNode.AppendChild(lang);

                doc.Save(XMLFilePath);
            }
            catch
            {

            }
        }

        public void UpdateSettingsValue(string value, [CallerMemberName] string nodeName = "")
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(XMLFilePath);

                XmlNode node = xmlDoc.SelectSingleNode("Settings/" + nodeName);

                node.Attributes["value"].Value = value;
                xmlDoc.Save(XMLFilePath);
            }
            catch
            {

            }
        }

        public string ReadSettingsValue([CallerMemberName] string nodeName = "")
        {
            try
            {
                string val = "";

                using (XmlReader xmlReader = XmlReader.Create(XMLFilePath))
                {
                    while (xmlReader.Read())
                    {
                        if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == nodeName))
                        {
                            val = xmlReader.GetAttribute("value");
                        }
                    }
                }

                return val;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
