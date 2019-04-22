using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsBL.Model
{
    /// <summary>
    /// Класс описывающий затраты на сотрудников, обслуживающих продукт
    /// </summary>
    public class EmployeeCosts
    {
        [Key]
        public int id { get; set; }
        public double? Salary { get; set; }
        public double? Training { get; set; }
        public int? idLink { get; set; }

        public virtual Product Products { get; set; }
    }
}
