using MvvmHelpers;
using System.Collections.Generic;

namespace StraviaTecMovil.Models
{
    public class HomeModel : BaseModel
    {
        public ObservableRangeCollection<MenuItem> MenuItems { get; }

        public HomeModel()
        {
            MenuItems = new ObservableRangeCollection<MenuItem>();
        }

        public void AddItems(ICollection<MenuItem> items)
        {
            MenuItems.ReplaceRange(items);
        } 
    }
}
