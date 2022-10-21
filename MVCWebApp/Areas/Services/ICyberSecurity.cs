using System.Threading.Tasks;

namespace MVCWebApp.Areas.Services
{
    public interface ICyberSecurity
    {
        string Decrypt(string value);
        Task<string> Encrypt(string value);
        string GetConnectionString(string value);

    }
}
