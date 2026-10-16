namespace RaceDayAPI.Models
{//start of namespace
    public class Category
    {//start of class
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
        public string CategoryValue { get; set; } = string.Empty;
    }//end of class
}//end of namespace
