using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraviaTecMovil.Models
{
    public class ActivitiesModel: BaseModel
    {
        private bool isRefreshingLocal;
        private bool isRefreshingAll;

        public bool IsRefreshingLocal
        {
            get => isRefreshingLocal;
            set
            {
                isRefreshingLocal = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsRefreshingAll
        {
            get => isRefreshingAll;
            set
            {
                isRefreshingAll = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableRangeCollection<Actividad> LocalActivities { get; set; }
        public ObservableRangeCollection<Actividad> AllActivities { get; set; }

        public ActivitiesModel()
        {
            LocalActivities = new ObservableRangeCollection<Actividad>();
            AllActivities = new ObservableRangeCollection<Actividad>();
        }
    }
}
