using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication_v1._0
{
    public class DbInitializer : DropCreateDatabaseAlways<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            var days = new List<Day>
        {
            new Day(){ ID=1, Date=new DateTime(2023,4,27,10,0,0), Count = 0},
        };
            days.ForEach(s => context.Days.Add(s));

            var tasks = new List<Task>
        {
            new Task() { ID = 10001, Date = new DateTime(2023, 4, 27, 10, 0, 0), Status = false, Priority = 10, DayID = 1, Description = "sample task 1" },
            new Task() { ID = 10002, Date = new DateTime(2023, 4, 28, 10, 0, 0), Status = true, Priority = 3, DayID = 1, Description = "sample task 2" },
            new Task() { ID = 10003, Date = new DateTime(2023, 4, 29, 10, 0, 0), Status = false, Priority = 20, DayID = 1, Description = "sample task 3" },
        };

            tasks.ForEach(s => context.Tasks.Add(s));

            context.SaveChanges();
        }
    }
}
