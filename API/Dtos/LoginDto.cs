using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Você deve informar o seu login de acesso")]
        public string LoginUser { get; set; }
        [Required(ErrorMessage = "Você deve informar sua senha de acesso")]
        public string Password { get; set; }
    }
}
