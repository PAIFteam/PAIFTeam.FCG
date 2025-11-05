using MediatR;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Games.DTOs;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Games.Queries
{
    public class GetGameByUIdQuery : IRequest<GameDto>
    {
        public GetGameByUIdQuery(Guid gameUId)
        {
            GameUId = gameUId;
        }
        public Guid GameUId { get; private set; }

        private Game _game;
        public Game GetGame() => _game;
        public void SetGame(Game game) => _game = game;

    }

    public class GetGameByUIdQueryHandler : IRequestHandler<GetGameByUIdQuery, GameDto>
    {
        private readonly IGameReadOnlyRepository _gameReadOnlyRepository;

        public GetGameByUIdQueryHandler(IGameReadOnlyRepository gameReadOnlyRepository)
        {
            _gameReadOnlyRepository = gameReadOnlyRepository;
        }

        public async Task<GameDto> Handle(GetGameByUIdQuery request, CancellationToken cancellationToken)
        {
            var game = request.GetGame();
            var gameDto = new GameDto();

            if (game != null)
            {
                gameDto.UId = game.UId;
                gameDto.Name = game.Name;
                gameDto.Value = game.Value.ToString("F2");
            }
            return gameDto;
        }
    }
}