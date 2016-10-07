using FluentValidation;
using FluentValidation.Attributes;
using Smart.Core.Extensions;

namespace Smart.Sample.Web.Models
{
    [Validator(typeof(SignInModelValidator))]
    public class SignInModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            //RuleFor(u => u.LoginName).NotEmpty().WithMessage("Login name can not be empty!".T());
            //RuleFor(u => u.Password).NotEmpty().WithMessage("Login password can not be empty!".T());
        }
    }
}