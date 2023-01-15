using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//добавление класса со свойствами, которые соответсвуют полям данных в таблице для сопоставления объектов и таблиц в бд 
namespace KES
{
    internal class customer
    {
        public string id { get; set; }
        public string name_of { get; set; }
        public string podrazd_of { get; set; }

    }
}
