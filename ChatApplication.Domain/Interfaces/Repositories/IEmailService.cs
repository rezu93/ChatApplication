namespace ChatApplication.Domain
{
    public interface IEmailService
    {
        void SendEmail(string email, string message);
    }
}
