namespace Paybliss.Repository
{
    public interface IFlightService
    {
        Task SearchFlight();
        Task GetFlightId(string id);

    }
}
