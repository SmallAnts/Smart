using FluentValidation;
using System;

namespace Smart.Samples.Domain.Validators
{
    public class UserInfoValidator : AbstractValidator<Entites.UserInfo>
    {
        public UserInfoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("用户名称不能为空！");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("邮箱不能为空！")
                .EmailAddress().WithMessage("邮箱格式不正确！");
        }
    }
}
