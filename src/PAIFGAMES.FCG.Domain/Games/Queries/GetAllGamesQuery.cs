using MediatR;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Games.DTOs;
using PAIFGAMES.FCG.Domain.Games.Filter;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Games.Queries
{
    public class GetAllGamesQuery : IRequest<List<GameDto>>
    {
    }

    public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, List<GameDto>>
    {
        private readonly IGameReadOnlyRepository _gameReadOnlyRepository;

        public GetAllGamesQueryHandler(IGameReadOnlyRepository gameReadOnlyRepository)
        {
            _gameReadOnlyRepository = gameReadOnlyRepository;
        }

        public async Task<List<GameDto>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            var gamesCustom = await _gameReadOnlyRepository.GetAllGamesAsync(new GameFilterModel(), new PageFilterModel(), cancellationToken);
            var games = new List<GameDto>();

            foreach (var game in gamesCustom)
            {
                games.Add(new GameDto
                {
                    UId = game.UId,
                    Name = game.Name,
                    Value = game.Value.ToString("F2")
                });
            }
            return games;
        }
    }
}