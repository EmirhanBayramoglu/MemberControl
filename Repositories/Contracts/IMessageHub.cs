namespace Repositories.Contracts
{
    public interface IMessageHub
    {
        Task SendAddMessage(string userName);
    }
}
