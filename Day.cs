using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication_v1._0
{
    public class Day
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Task> Task { get; set; }
    }
}
