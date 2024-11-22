using System.Security.Claims;

namespace Contract_Monthly_Claim_System.Models
{
    public class Lecturer
    {
        public int LecturerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Claim> Claims { get; set; } // A Lecturer can have multiple Claims
    }
}


