namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        //Get ==> Already Cached [ Return data ] ==> response chacing
        Task<string?>  GetAsync(string key);

        //Set ==> Not Cached [ first time to call endpoint ] ==> Cache data
        Task SetAsync(string key, object value, TimeSpan duration);
    }
}
