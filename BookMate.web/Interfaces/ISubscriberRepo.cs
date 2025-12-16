namespace BookMate.web.Interfaces
{
    public interface ISubscriberRepo : IGenericRepo<Subscriber>
    {
        Task<SubscriberFormViewModel> PopulateViewModel(SubscriberFormViewModel? model=null);
        Task<Subscriber?> GetSubscriber(string searchTerm);
        DateTime RenewSubscription(int subscriberId, string UserId);

        Task<List<Area>> GetGovernorateAreas(int id);
        bool AllowNationalId(string NationalId);
        bool AllowMobileNumber(string MobileNumber);
        bool AllowEmail(string Email);

    }
}
