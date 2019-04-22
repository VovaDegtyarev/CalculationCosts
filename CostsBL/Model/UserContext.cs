using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsBL.Model
{
    /// <summary>
    /// Класс для подключения к БД
    /// </summary>
    public class UserContext : DbContext
    {
        //не забыть поменять в самом конце путь создания бд на "data source=(localdb)\MSSQLLocalDB"
        public UserContext() : base("DBConnSt")
        {

        }

        public DbSet<Registration> RegistrationsTable { get; set; }
        public DbSet<Product> ProductsTable { get; set; }
        public DbSet<CostPrice> CostPricesTable { get; set; }
        public DbSet<EmployeeCosts> EmoloyeeCostsTable { get; set; }


    }
}
