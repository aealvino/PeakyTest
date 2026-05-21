namespace PeakyStart.Domain.Interfaces.Services
{
    public interface ISettingsService
    {
        void SaveLastSession();
        DateTimeOffset? LoadLastSession();
    }
}
