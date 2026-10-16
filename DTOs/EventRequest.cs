namespace RaceDayAPI.DTOs
{//start of the namespace
    public class EventRequest
    {//start of class
        public int EventTypeID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Distance { get; set; }

    }//end of class
}//end of namespace
