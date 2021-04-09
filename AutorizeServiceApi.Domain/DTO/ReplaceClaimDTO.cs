using System.Security.Claims;

namespace AutorizeServiceApi.Domain.DTO
{
    public class ReplaceClaimDTO : UserDTO
    {
        public Claim Claim { get; set; }
        public Claim NewClaim { get; set; }
    }
}