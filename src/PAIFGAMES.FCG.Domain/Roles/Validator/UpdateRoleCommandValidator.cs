using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Roles.Commands;
using System.Security;

namespace PAIFGAMES.FCG.Domain.Roles.Validator
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator(RoleManager<IdentityRoleCustom> roleManager)
        {
            RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x)).WithMessage("O campo Name é obrigatório.").WithName("Name");
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
                    var role = roleManager.Roles.FirstOrDefault(x => x.Name == a.Name);
                    if (role?.UId == a.RoleUId || role == null)
                    {
                        return true;
                    }
                    return false;
                }).WithMessage("Um perfil com esse nome já existe.").WithName("Name");
            });
        }
    }
}
