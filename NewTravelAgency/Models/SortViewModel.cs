namespace NewTravelAgency.Models
{
    public class SortViewModel
    {
        public SortTicket HotelNameSort { get; } // значення для сортування по імені готелю
        public SortTicket CostSort { get; }    // значення для сортування по ціні квитка
        public SortTicket Current { get; }     // поточне значення сортування

        public SortViewModel(SortTicket sortOrder)
        {
            HotelNameSort = sortOrder == SortTicket.HotelNameAsc ? SortTicket.HotelNameDesc : SortTicket.HotelNameAsc;
            CostSort = sortOrder == SortTicket.CostAsc ? SortTicket.CostDesc : SortTicket.CostAsc;
            Current = sortOrder;
        }

    }
}
