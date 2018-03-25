using System.Threading.Tasks;

namespace Kuaiyipai.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}