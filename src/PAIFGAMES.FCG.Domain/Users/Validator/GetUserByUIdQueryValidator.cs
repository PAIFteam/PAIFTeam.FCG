using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Users.Queries;

namespace PAIFGAMES.FCG.Domain.Users.Validator
{
    public class GetUserByUIdQueryValidator : AbstractValidator<GetUserByUIdQuery>
    {
        public GetUserByUIdQueryValidator(UserManager<IdentityUserCustom> userManager)
        {
            RuleFor(x => x.UserUId)
                .Must(x => x != Guid.Empty).WithMessage("O campo UserUId é obrigatório.");
            When(x => x.UserUId != Guid.Empty, () =>
            {
                RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .Must(a =>
                {
                    var user = userManager.Users.FirstOrDefault(x => x.UId == a.UserUId);
                    if (user != null)
                    {
                        a.SetIdentityUserCustom(user);
                        return true;
                    }
                    return false;
                }).WithMessage("Usuário não encontrado.").WithName("UserUId");
            });
        }
    }
}
