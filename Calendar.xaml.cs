using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TodoApplication_v1._0;

namespace TodoApplication_v1._0
{
    public partial class Calendar : UserControl
    {
        private ContentControl contentControl;
        Button buttonDeleteTask;
        public Calendar(ContentControl contentControl)
        {
            InitializeComponent();
            calendar.SelectedDate = DateTime.Now; // Ustawia dzisiejszą date w kontrolce kalendarza
            this.contentControl = contentControl;

            if (calendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = calendar.SelectedDate.Value.Date;

                using (var context = new AppContext())
                {
                    var tasks = context.Tasks // Wyswietla zadania w zależności od wciśniętego dnia - domyślnie aktualny dzień
                        .Where(t => DbFunctions.TruncateTime(t.Date) == selectedDate.Date)
                        .ToList();

                    taskList.ItemsSource = tasks;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new AddTask(contentControl); // Utworzenie nowego widoku
            contentControl.Content = addTask; // Przypisanie do kontenera nowego widoku
        }

        private void DeleteTask(int taskId) // Funkcja pomocnicza do usuwania zadań
        {
            using (var context = new AppContext())
            {
                var taskToRemove = context.Tasks.FirstOrDefault(t => t.ID == taskId);
                if (taskToRemove != null)
                {
                    context.Tasks.Remove(taskToRemove);
                    context.SaveChanges();
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var task = (Task)checkBox.DataContext;

            task.Status = checkBox.IsChecked ?? false; // Aktualizuje status w obiekcie Task

            using (var context = new AppContext())
            {
                var existingTask = context.Tasks.Find(task.ID); // Znajdź obiekt Task w kontekście bazy danych
                if (existingTask != null)
                {
                    existingTask.Status = task.Status; // Zmienia status w bazie danych
                    context.SaveChanges(); // Zapisuje zmiany
                }
            }
        }

        private void Button_Click_1(object sender, EventArgs e) // Usuwanie zadań
        {
            if (taskList.SelectedItems.Count > 0) // Jeśli jakieś zadanie jest zaznaczone
            {
                object selectedItem = taskList.SelectedItems[0];
                if (selectedItem is Task selectedTask)
                {
                    int taskId = selectedTask.ID;
                    int dayId = selectedTask.DayID; // Pobiera identyfikator dnia z zadania

                    DeleteTask(taskId); // Usunięcie zadania z bazy danych

                    if (dayId != 0) // Jeśli zadanie było przypisane do konkretnego dnia
                    {
                        using (var context = new AppContext())
                        {
                            Day day = context.Days.Include(d => d.Task).FirstOrDefault(d => d.ID == dayId);
                            if (day != null)
                            {
                                taskList.ItemsSource = day.Task.ToList(); // Aktualizacja listy zadań bez usuniętego dla danego dnia
                            }
                            else
                            {
                                taskList.ItemsSource = null; // Brak zadań dla znalezionego dnia
                            }
                        }
                    }
                    else
                    {
                        taskList.ItemsSource = null; // Zadanie nie było przypisane do konkretnego dnia
                    }
                }
            }
        }

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = calendar.SelectedDate.Value.Date; // Zczytuje aktualna wybraną date

                using (var context = new AppContext()) // Wyświetla zadania z danego dnia w taskList
                {
                    var days = context.Days.Include(d => d.Task).ToList();
                    Day day = days.FirstOrDefault(d => d.Date.Date == selectedDate);

                    if (day != null)
                    {
                        taskList.ItemsSource = day.Task.ToList();
                    }
                    else
                    {
                        taskList.ItemsSource = null;
                    }
                }
            }
        }
    }
}
