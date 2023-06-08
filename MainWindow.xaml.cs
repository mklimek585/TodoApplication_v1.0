using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TodoApplication_v1._0
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var context = new AppContext();

            var days = new List<Day>
            {
                new Day(){ ID=1, Date=new DateTime(2023,4,27,10,0,0), Count = 0},
            };
            days.ForEach(s => context.Days.Add(s));
            context.SaveChanges();

            var tasks = new List<Task>
            {
                new Task() { ID=10001, Date=new DateTime(2023,4,27,10,0,0), Status = false, Priority = 10, DayID = 1, Description = "sample task 1" },
                new Task() { ID=10002, Date=new DateTime(2023,4,28,10,0,0), Status = true, Priority = 3, DayID = 1, Description = "sample task 2" },
                new Task() { ID=10003, Date=new DateTime(2023,4,29,10,0,0), Status = false, Priority = 20, DayID = 1, Description = "sample task 3" },
            };
            //tasks.ForEach(s => context.Tasks.Add(s));
            //context.SaveChanges();


            Calendar calendar = new Calendar(contentControl);

            contentControl.Content = calendar;
        }
    }

}
