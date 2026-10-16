namespace RaceDayAPI.DTOs
{//start of namespace
    public class CategoryRequest
    {//start of class
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
        public string CategoryValue { get; set; } = string.Empty;
    }//end of class
}//end of namespace

