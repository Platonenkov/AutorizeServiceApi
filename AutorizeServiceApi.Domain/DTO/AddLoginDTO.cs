using Microsoft.AspNetCore.Identity;

namespace AutorizeServiceApi.Domain.DTO
{
    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }
}