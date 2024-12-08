using Finanzia.Domain.Entities;

namespace Finanzia.Application.Contract
{
    public interface IJwtTokenService
    {
        string GenerarToken(Usuario usuario);
    }
}

