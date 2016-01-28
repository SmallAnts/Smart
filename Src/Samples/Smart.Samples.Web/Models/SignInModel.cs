using FluentValidation;
using FluentValidation.Attributes;

namespace Smart.Samples.Web.Models
{
    [Validator(typeof(SignInModelValidator))]
    public class SignInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("邮箱不能为空！")
                .EmailAddress().WithMessage("邮箱格式不正确！");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("登录密码不能为空！");
        }
    }
}