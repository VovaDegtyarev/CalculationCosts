using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsBL.Model
{

    /// <summary>
    /// Класс авторизации пользователя
    /// </summary>
    public class Registration
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }



    }
}
