using API.Dtos;

namespace API.Models
{
    public class ReturnLogin
    {
        public UserViewDto UserView { get; set; }
        public string Token { get; set; }
    }
}
