using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Roles.Commands;

namespace PAIFGAMES.FCG.Domain.Roles.Validator
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator(RoleManager<IdentityRoleCustom> roleManager, UserManager<IdentityUserCustom> userManager)
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

            When(x => x.RoleUId != Guid.Empty && x.GetIdentityRoleCustom() != null, () =>
            {
                RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .Must(a =>
                {
                    var users = userManager.GetUsersInRoleAsync(a.GetIdentityRoleCustom().NormalizedName!).GetAwaiter().GetResult();
                    if (users != null)
                    {
                        return true;
                    }
                    return false;
                }).WithMessage("O perfil não pode ser removido enquanto estiver sendo utilizado.").WithName("RoleUId");
            });



        }
    }
}