using System;
namespace MovieLand.Models
{
    public class AppSettings
    {
        public AdminCredentials AdminCredentials { get; set; }   
    }

    public class AdminCredentials 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
