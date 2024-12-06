using Domain.Entities;

namespace Application.Interfaces.HelperInterfaces
{
    public interface ITokenHelper
    {
        string GenerateJwtToken(User user);
    }
}
