namespace RaceDayAPI.DTOs
{
    public class CategoryRequest
    {
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
        public string CategoryValue { get; set; } = string.Empty;
    }
}

