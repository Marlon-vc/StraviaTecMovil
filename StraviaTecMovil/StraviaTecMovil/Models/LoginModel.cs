using System;
using System.Collections.Generic;
using System.Text;

namespace StraviaTecMovil.Models
{
    public class LoginModel : BaseModel
    {
        private bool busy;
        private string pass;
        private string user;

        public string User { get => user; set { user = value; NotifyPropertyChanged(); } }
        public string Pass { get => pass; set { pass = value; NotifyPropertyChanged(); } }
        public bool Busy { get => busy; set { busy = value; NotifyPropertyChanged(); } }

        public LoginModel()
        {
            User = "";
            Pass = "";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Pass);
        }

    }
}
