namespace RaceDayAPI.Models
{//start of namespace
    public class User
    {//start of class
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;

    }//end of class
}//end of namepsace
