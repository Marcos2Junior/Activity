using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserInsertDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Nome deve ter tamanho entre 4 e 20 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Numero de telefone é obrigatório")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6 , ErrorMessage = "Senha deve ter tamanho entre 6 e 20 caracteres")]
        public string Password { get; set; }
    }
}
