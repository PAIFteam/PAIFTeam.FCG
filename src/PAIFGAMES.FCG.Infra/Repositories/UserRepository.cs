using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Context;

namespace PAIFGAMES.FCG.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<IdentityUserCustom> userManager,
            RoleManager<IdentityRole<long>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AttachGameToUserAsync(IdentityUserCustom user, Game game, CancellationToken cancellationToken)
        {
            var userGame = new UserGame
            {
                UserId = user.Id,
                GameId = game.Id
            };

            await _context.UserGames.AddAsync(userGame, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DetachGameFromUserAsync(IdentityUserCustom user, Game game, CancellationToken cancellationToken)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.GameId == game.Id, cancellationToken);

            if (userGame != null)
            {
                _context.UserGames.Remove(userGame);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task AttachRoleToUserAsync(IdentityUserCustom user, string roleNormalizedName, CancellationToken cancellationToken)
        {
            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.NormalizedName == roleNormalizedName, cancellationToken);

            if (role == null)
                throw new InvalidOperationException($"Role '{roleNormalizedName}' não encontrada.");

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Erro ao adicionar role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        public async Task DetachRoleFromUserAsync(IdentityUserCustom user, string roleNormalizedName, CancellationToken cancellationToken)
        {
            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.NormalizedName == roleNormalizedName, cancellationToken);

            if (role == null)
                throw new InvalidOperationException($"Role '{roleNormalizedName}' não encontrada.");

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Erro ao remover role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}
