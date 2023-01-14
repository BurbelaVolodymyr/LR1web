using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewTravelAgency.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Hotel> hotels, int hotel)
        {
            // встановлюємо початковий елемент який дозволить вибрати всіх
            hotels.Insert(0, new Hotel { Name = "All", Id =0 });
            Hotels = new SelectList(hotels, "Id", "Name", "hotel");
            SelectedHotel = hotel;
            
        }
        public SelectList Hotels { get; } // список готелів
        public int SelectedHotel { get; } // вибраний готель

    }
}
