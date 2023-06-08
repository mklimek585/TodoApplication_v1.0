using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TodoApplication_v1._0
{
    public class Task : INotifyPropertyChanged
    {
        private bool _status;

        public bool Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }

        public int DayID { get; set; }
        public virtual Day Day { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
