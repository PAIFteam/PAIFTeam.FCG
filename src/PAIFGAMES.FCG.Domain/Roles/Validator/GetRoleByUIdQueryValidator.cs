using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Roles.Queries;

namespace PAIFGAMES.FCG.Domain.Roles.Validator
{
    public class GetRoleByUIdQueryValidator : AbstractValidator<GetRoleByUIdQuery>
    {
        public GetRoleByUIdQueryValidator(RoleManager<IdentityRoleCustom> roleManager)
        {
            RuleFor(x => x.RoleUId)
                .Must(x => x != Guid.Empty).WithMessage("O campo RoleUId é obrigatório.");
            When(x => x.RoleUId != Guid.Empty, () =>
            {
                RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .Must(a =>
                {
                    var role = roleManager.Roles.FirstOrDefault(x => x.UId == a.RoleUId);
                    if (role != null)
                    {
                        a.SetIdentityRoleCustom(role);
                        return true;
                    }
                    return false;
                }).WithMessage("Perfil não encontrado.").WithName("RoleUId");
            });
        }
    }
}
