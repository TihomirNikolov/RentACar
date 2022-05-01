using System.Collections.Generic;

namespace Client.BussinesModels
{
    public class Hour
    {
        public string HourString { get; set; }

        public Hour(string hour)
        {
            HourString = hour;
        }

        public static List<Hour> LoadHoursList()
        {
            List<Hour> HoursList = new List<Hour>();

            HoursList.Add(new Hour("0:00"));
            HoursList.Add(new Hour("1:00"));
            HoursList.Add(new Hour("2:00"));
            HoursList.Add(new Hour("3:00"));
            HoursList.Add(new Hour("4:00"));
            HoursList.Add(new Hour("5:00"));
            HoursList.Add(new Hour("6:00"));
            HoursList.Add(new Hour("7:00"));
            HoursList.Add(new Hour("8:00"));
            HoursList.Add(new Hour("9:00"));
            HoursList.Add(new Hour("10:00"));
            HoursList.Add(new Hour("11:00"));
            HoursList.Add(new Hour("12:00"));
            HoursList.Add(new Hour("13:00"));
            HoursList.Add(new Hour("14:00"));
            HoursList.Add(new Hour("15:00"));
            HoursList.Add(new Hour("16:00"));
            HoursList.Add(new Hour("17:00"));
            HoursList.Add(new Hour("18:00"));
            HoursList.Add(new Hour("19:00"));
            HoursList.Add(new Hour("20:00"));
            HoursList.Add(new Hour("21:00"));
            HoursList.Add(new Hour("22:00"));
            HoursList.Add(new Hour("23:00"));

            return HoursList;
        }
    }
}
