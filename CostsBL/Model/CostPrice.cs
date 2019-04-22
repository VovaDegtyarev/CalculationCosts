using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsBL.Model
{
    /// <summary>
    /// Класс описывающий затраты на продукт
    /// </summary>
    public class CostPrice
    {   
        [Key]
        public int Id { get; set; }

        public double? MaterilaCost { get; set; }

        public double? SemiFinishedProducts { get; set; }

        public double? FuelAndEnergyCosts { get; set; }

        public double? Depreciation { get; set; }

        public double? Insurance { get; set; }

        public double? Transport { get; set; }

        public double? Sales { get; set; }

        public double? Other { get; set; }

        public int? idLink { get; set; }

        public virtual Product Products { get; set; }



    }
}
