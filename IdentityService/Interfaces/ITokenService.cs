using IdentityService.Models;

namespace IdentityService.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}