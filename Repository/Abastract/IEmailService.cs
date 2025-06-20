namespace Eletronic_Api.Repository.Abastract
{
    public interface IEmailService
    {
        void SendOtp(string email, string otp);
    }
}
