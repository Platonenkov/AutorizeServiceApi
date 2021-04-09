using AutorizeServiceApi.Domain.Models;

namespace AutorizeServiceApi.Domain.DTO
{
    public abstract class UserDTO
    {
        public ApplicationUser User { get; set; }
    }
}