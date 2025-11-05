using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Users.Commands;

namespace PAIFGAMES.FCG.Domain.Users.Validator
{
    public class DetachRoleUserCommandValidator : AbstractValidator<DetachRoleUserCommand>
    {
        public DetachRoleUserCommandValidator(UserManager<IdentityUserCustom> userManager, RoleManager<IdentityRoleCustom> roleManager)
        {
            RuleFor(x => x.UserUId)
                .Must(x => x != Guid.Empty).WithMessage("O campo UserUId é obrigatório.");
            RuleFor(x => x.RoleUId)
                .Must(x => x != Guid.Empty).WithMessage("O campo RoleUId é obrigatório.");

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

            When(x => x.RoleUId != Guid.Empty && x.GetIdentityUserCustom() != null, () =>
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

            When(x => x.UserUId != Guid.Empty && x.GetIdentityUserCustom() != null, () =>
            {
                RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .Must(a =>
                {
                    var roles = userManager.GetRolesAsync(a.GetIdentityUserCustom()).GetAwaiter().GetResult();
                    if (roles?.Count < 1)
                    {
                        return false;
                    }
                    return true;
                }).WithMessage("Usuário não possui perfil.").WithName("UserUId");
            });

            
        }
    }
}
