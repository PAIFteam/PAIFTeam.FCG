using FluentValidation;
using PAIFGAMES.FCG.Domain.Users.Queries;

namespace PAIFGAMES.FCG.Domain.Users.Validator
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            
        }
    }
}
