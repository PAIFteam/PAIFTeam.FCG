using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PAIFGAMES.FCG.Infra.Auth.Jwt.Model
{
    public static class ClaimExtensions
    {
        public static void RemoveRefreshToken(this ICollection<Claim> claims)
        {
            var refreshTokenClaim = claims.FirstOrDefault(c => c.Type == "LastRefreshToken");
            if (refreshTokenClaim != null)
            {
                claims.Remove(refreshTokenClaim);
            }
        }
    }
}
