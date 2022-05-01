using Database.Models;

namespace Client.BussinesModels
{
    public class CarPeriodWrapper
    {
        #region Declaration

        public Car Car { get; set; }
        public Period Period { get; set; }
        public LocationWrapper LocationFrom { get; set; }
        public LocationWrapper LocationTo { get; set; }

        #endregion

        public CarPeriodWrapper(Car car, Period period, LocationWrapper locationFrom, LocationWrapper locationTo)
        {
            Car = car;
            Period = period;
            LocationFrom = locationFrom;
            LocationTo = locationTo;
        }

        public decimal TotalPrice
        {
            get
            {
                return Car.Price * Days;
            }
        }

        public int Days
        {
            get
            {
                if(Period.StartPeriod.Hour == Period.EndPeriod.Hour)
                {
                    return (Period.EndPeriod - Period.StartPeriod).Days;
                }
                return (Period.EndPeriod - Period.StartPeriod).Days + 1;
            }
        }
    }
}
