using System;

namespace AutorizeServiceApi.Domain.DTO
{
    public class SetLockoutDTO : UserDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}