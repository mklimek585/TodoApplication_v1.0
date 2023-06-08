using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoApplication_v1._0
{
    public partial class AddTask : UserControl
    {
        private ContentControl contentControl;
        public AddTask(ContentControl contentControl)
        {
            InitializeComponent();
            this.contentControl = contentControl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calendar calendar = new Calendar(contentControl); // Utworzenie nowego widoku
            contentControl.Content = calendar; // Przypisanie do kontenera nowego widoku
        }
        private int GetNextTaskId() // Funkcja pomocnicza zwracająca wartość następnego ID zadania
        {
            using (var context = new AppContext())
            {
                if (context.Tasks.Any())
                {
                    int maxId = context.Tasks.Max(t => t.ID);
                    return maxId + 1;
                }
                else
                {
                    // Jeśli baza danych jest pusta, zwróć wartość początkową dla ID
                    return 1000;
                }
            }
        }

        private int GetOrCreateDay(DateTime date) // Funkcja pomocnicza która, jeśli nie ma danego dnia w obiekcie Days - tworzy go,
        {                                         //  jeśli jest to zwraca ID odpowiadające danej dacie
            var dateOnly = date.Date;  
            using (var context = new AppContext())
            {
                var day = context.Days.FirstOrDefault(d => d.Date == dateOnly);
                if (day == null)
                {
                    day = new Day { Date = dateOnly };
                    context.Days.Add(day);
                    context.SaveChanges();
                }
                return day.ID;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Funkcja dodająca nowe zadanie z podstawową obsługą błędów - 
        {                                                             // wymaga podania wszystkich parametrów
            // Pobierz dane z kontrolki DatePicker
            DateTime taskDate = var_date.SelectedDate ?? DateTime.Now;
            if (taskDate == null || taskDate == DateTime.MinValue)
            {
                MessageBox.Show("Proszę wybrać datę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz dane z kontrolki TextBox - opis
            string taskDescription = var_desc.Text;
            if (string.IsNullOrWhiteSpace(taskDescription))
            {
                MessageBox.Show("Proszę wprowadzić opis zadania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Pobierz dane z kontrolki TextBox - priorytet
            int taskPriority;
            bool isPriorityValid = int.TryParse(var_prio.Text, out taskPriority);
            if (!isPriorityValid || taskPriority < 0)
            {
                MessageBox.Show("Proszę wprowadzić prawidłowy priorytet zadania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Przypisanie pobranych wartości do bazy danych poprzez obiekt Task
            int taskId = GetNextTaskId();
            int dayId = GetOrCreateDay(taskDate); // Stworzenie lub pobranie dayID odpowiadającego dla danego dnia
            var newTask = new Task()
            {
                ID = taskId,
                Date = taskDate,
                Status = false,
                Priority = taskPriority,
                DayID = dayId, // Połączenie Tasku z danym dniem
                Description = taskDescription
            };

            // Dodaj nowe zadanie do bazy danych
            using (var context = new AppContext())
            {
                context.Tasks.Add(newTask);
                context.SaveChanges();
            }

            // Czyszczenie TextBoxow 
            var_desc.Text = string.Empty;
            var_prio.Text = string.Empty;

            Calendar calendar = new Calendar(contentControl); // Utworzenie nowego widoku
            contentControl.Content = calendar; // Przypisanie do kontenera nowego widoku
        }
    }
}

