using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}