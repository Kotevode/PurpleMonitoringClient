using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using static PurpleMonitoringClient.Client.JobStatusChanged;

namespace PurpleMonitoringClient.ViewModel
{
    public class JobViewModel : INotifyPropertyChanged
    {
        int weight;
        public int Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("BlockHeight");
            }
        }
        
        public int MaxLoad { get; private set; }

        public double BlockHeight
        {
            get { return (double)Weight / MaxLoad * 300;  }
        }

        JobStatus status = JobStatus.Waiting;
        public JobStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public Color BackgroundColor
        {
            get
            {
                switch (Status)
                {
                    case JobStatus.Running:
                        return Colors.Yellow;
                    case JobStatus.Done:
                        return Colors.Green;
                    case JobStatus.Error:
                        return Colors.Red;
                    default:
                        return Colors.White;
                }
            }
        }
        public int Index { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public JobViewModel(int index, int weight, int maxLoad)
        {
            Index = index;
            MaxLoad = maxLoad;
            Weight = weight;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
