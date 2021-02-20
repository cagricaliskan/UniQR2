using System.Threading.Tasks;

namespace UniQR2.Services
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string html, string from = null);
    }
}
