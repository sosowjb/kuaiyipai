using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
