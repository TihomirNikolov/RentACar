using Database.Enums;
using System;

namespace Client.BussinesModels
{
    public class Language
    {
        public LanguageEnum Lang { get; set; }
        public string IconPath { get; set; }

        public Language(string Language, string IconPath)
        {
            Lang = (LanguageEnum)Enum.Parse(typeof(LanguageEnum),Language);
            this.IconPath = IconPath;
        }
    }
}
