namespace AssignmentPRN222.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProvince Provinces { get; }
        ICinema Cinemas { get; }
        IMovie Movies { get; }
        IShowTime ShowTimes { get; }
        ISeat Seats { get; }
        IRoom Rooms { get; }
        ISeatBooking SeatBookings { get; }
        IDiscount Discounts { get; }
        IOrder Orders { get; }
        IUser User { get; }
        int Complete();
    }
}
