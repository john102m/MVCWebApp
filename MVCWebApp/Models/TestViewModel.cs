using System.ComponentModel;

namespace MVCWebApp.Models
{
    public class TestViewModel
    {
        [DisplayName("Your Test String: ")]
        public string TestString { get; set; }
        public int RandomNumber { get; set; }
    }
}
