using MediatR;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Domain.Users.DTOs;
using PAIFGAMES.FCG.Domain.Users.Filter;

namespace PAIFGAMES.FCG.Domain.Users.Queries
{
    public class GetAllUsersQuery : IRequest<PagedList<UserDto>>
    {
        public UserFilterModel UserFilter { get; set; }
        public PageFilterModel PaginationFilter { get; set; }
        public GetAllUsersQuery(UserFilterModel userFilter, PageFilterModel paginationFilter)
        {
            UserFilter = userFilter;
            PaginationFilter = paginationFilter;
        }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserDto>>
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public GetAllUsersQueryHandler(IUserReadOnlyRepository userReadOnlyRepository)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<PagedList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userReadOnlyRepository.GetFilteredUsersAsyncWithoutLockout(request.UserFilter, request.PaginationFilter, cancellationToken);
        }
    }
}
