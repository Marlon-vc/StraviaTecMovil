using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace StraviaTecMovil.Models
{
    public class RecordActivityModel : BaseModel
    {
        private int currentColspan;
        private string currentAction;
        private string elapsedTime;
        private bool onActivity;

        public string ElapsedTime
        {
            get => elapsedTime;
            set
            {
                elapsedTime = value;
                NotifyPropertyChanged();
            }
        }
        public string CurrentAction
        {
            get => currentAction;
            set
            {
                currentAction = value;
                NotifyPropertyChanged();
            }
        }
        public int CurrentColspan
        {
            get => currentColspan;
            set
            {
                currentColspan = value;
                NotifyPropertyChanged();
            }
        }
        public bool OnActivity
        {
            get => onActivity;
            set
            {
                onActivity = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableRangeCollection<Position> Route;

        public RecordActivityModel()
        {
            ElapsedTime = "00:00";
            CurrentAction = "Iniciar";
            CurrentColspan = 2;
            Route = new ObservableRangeCollection<Position>();
        }

        public void AddPosition(Position pos)
        {
            Route.Add(pos);
        }
    }
}
