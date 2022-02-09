using System;
using System.Collections.Generic;

namespace InterviewTask.Models
{
    public class HelperServiceModel
    {
        private string _openStatusText = String.Empty;
        private bool? _isOpen;

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TelephoneNumber { get; set; }
        public string WeatherInfo { get;set;}
        public string OpenStatusText
        { get
            {
                if(String.IsNullOrEmpty(_openStatusText))
                    UpdateOpenStatus();                

                return _openStatusText;
            } 

            private set
            {
                _openStatusText = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                if (!_isOpen.HasValue)
                    UpdateOpenStatus();
                
                return _isOpen.Value;
            }

            private set
            {
                _isOpen = value;
            }
        }
        public List<int> MondayOpeningHours { get; set; }        
        public List<int> TuesdayOpeningHours { get; set; }
        public List<int> WednesdayOpeningHours { get; set; }
        public List<int> ThursdayOpeningHours { get; set; }
        public List<int> FridayOpeningHours { get; set; }
        public List<int> SaturdayOpeningHours { get; set; }
        public List<int> SundayOpeningHours { get; set; }

        private void UpdateOpenStatus()
        {

            var noDataText = "We're sorry, we are temporarily unable to display opening times";
            var statusText = String.Empty;

            //Check if null data has been returned simulating the bug when data is not available for all days
            if (SundayOpeningHours == null || MondayOpeningHours == null || TuesdayOpeningHours == null || WednesdayOpeningHours == null
                || ThursdayOpeningHours == null || FridayOpeningHours == null || SaturdayOpeningHours == null)
            {
                OpenStatusText = noDataText;
                IsOpen = false;
                return;
            }

            //Check if open and then 
            var openingHour = 0;
            var closeingHour = 0;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    openingHour = SundayOpeningHours[0];
                    closeingHour = SundayOpeningHours[1];
                    break;
                case DayOfWeek.Monday:
                    openingHour = MondayOpeningHours[0];
                    closeingHour = MondayOpeningHours[1];
                    break;
                case DayOfWeek.Tuesday:
                    openingHour = TuesdayOpeningHours[0];
                    closeingHour = TuesdayOpeningHours[1];
                    break;
                case DayOfWeek.Wednesday:
                    openingHour = WednesdayOpeningHours[0];
                    closeingHour = WednesdayOpeningHours[1];
                    break;
                case DayOfWeek.Thursday:
                    openingHour = ThursdayOpeningHours[0];
                    closeingHour = ThursdayOpeningHours[1];
                    break;
                case DayOfWeek.Friday:
                    openingHour = FridayOpeningHours[0];
                    closeingHour = FridayOpeningHours[1];
                    break;
                case DayOfWeek.Saturday:
                    openingHour = SaturdayOpeningHours[0];
                    closeingHour = SaturdayOpeningHours[1];
                    break;
            }

            //Check if open now or open later today
            if (DateTime.Now.Hour >= openingHour && DateTime.Now.Hour < closeingHour)
                statusText = GetOpenText(closeingHour,DateTime.Now.DayOfWeek);
            else if (DateTime.Now.Hour < openingHour)
                statusText = GetClosedText(closeingHour, DateTime.Now.DayOfWeek);

            //Find next available open day and time
            if (String.IsNullOrEmpty(statusText))
            {
                //Start with tomorrow as we have checked today
                DayOfWeek nextDay = DateTime.Now.DayOfWeek;
                var nextOpen = 0;
                var nextClose = 0;

                if (nextDay == DayOfWeek.Saturday) 
                    nextDay = DayOfWeek.Sunday;
                else
                    nextDay = DateTime.Now.DayOfWeek + 1;

                //Loop for 7 days
                for (int i = 0; i < 7; i++)
                {
                    switch (nextDay)
                    {
                        case DayOfWeek.Sunday:
                            nextOpen = SundayOpeningHours[0];
                            nextClose = SundayOpeningHours[1];
                            break;
                        case DayOfWeek.Monday:
                            nextOpen = MondayOpeningHours[0];
                            nextClose = MondayOpeningHours[1];
                            break;
                        case DayOfWeek.Tuesday:
                            nextOpen = TuesdayOpeningHours[0];
                            nextClose = TuesdayOpeningHours[1];
                            break;
                        case DayOfWeek.Wednesday:
                            nextOpen = WednesdayOpeningHours[0];
                            nextClose = WednesdayOpeningHours[1];
                            break;
                        case DayOfWeek.Thursday:
                            nextOpen = ThursdayOpeningHours[0];
                            nextClose = ThursdayOpeningHours[1];
                            break;
                        case DayOfWeek.Friday:
                            nextOpen = FridayOpeningHours[0];
                            nextClose = FridayOpeningHours[1];
                            break;
                        case DayOfWeek.Saturday:
                            nextOpen = SaturdayOpeningHours[0];
                            nextClose = SaturdayOpeningHours[1];
                            break;
                    }

                    //If closed times return closed text
                    if (nextOpen != 0 && nextClose != 0)
                    {
                        statusText = GetClosedText(nextOpen, nextDay);
                        break; 
                    }

                    //Increment day
                    if (nextDay == DayOfWeek.Saturday)
                        nextDay = DayOfWeek.Monday;
                    else
                        nextDay++;
                }
            }

            //If for some reason data is not open
            if (String.IsNullOrEmpty(statusText))
                statusText = noDataText;

            OpenStatusText = statusText;

            //Set flag to update store open status
            if (OpenStatusText.Substring(0, 4).ToUpper() == "OPEN")
                IsOpen = true;
            else
                IsOpen = false; 
        }

        private string GetClosedText(int openHour, DayOfWeek day)
        {
            var dayText = "TODAY";
            if (day != DateTime.Now.DayOfWeek)
                dayText = day.ToString();
            return string.Concat("CLOSED - REOPENS ",dayText," at ", openHour, ":00");
        }

        private string GetOpenText(int closedHour, DayOfWeek day)
        {
            var dayText = "TODAY";
            if (day != DateTime.Now.DayOfWeek)
                dayText = day.ToString();
            return string.Concat("OPEN - OPEN ",dayText, " UNTIL ", closedHour, ":00");
        }
    }
}

