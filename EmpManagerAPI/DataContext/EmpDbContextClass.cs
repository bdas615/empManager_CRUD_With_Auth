using EmpManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpManagerAPI.DataContext
{
    public class EmpDbContextClass : DbContext
    {
        public EmpDbContextClass(DbContextOptions<EmpDbContextClass> options) : base(options) { }


        public DbSet<EmployeeParameters> EmployeeData { get; set; }
        public DbSet<Register> RegistrationData { get; set; }


    }
}
