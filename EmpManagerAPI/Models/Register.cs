using System.ComponentModel.DataAnnotations;

namespace EmpManagerAPI.Models
{
    public class Register
    {
        [Key]
        public int UserId { get; set; }
        public String Name { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String Pwd { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
