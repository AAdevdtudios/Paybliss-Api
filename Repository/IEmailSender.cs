namespace Paybliss.Repository
{
    public interface IEmailSender
    {
        public void SendEmail(string email, string otp);
    }
}
