using System.Collections.Generic;
using System.Security.Claims;

namespace AutorizeServiceApi.Domain.DTO
{
    public abstract class ClaimDTO : UserDTO
    {
        public IEnumerable<Claim> Claims { get; set; }

    }
}