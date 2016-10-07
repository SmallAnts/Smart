using FluentValidation;
using FluentValidation.Attributes;
using Smart.Core.Extensions;

namespace Smart.Sample.Core.Entites
{
    // 用户实体类扩展，public partial 开头，类名必须和表名一样
    [Validator(typeof(SysUserValidator))]
    public partial class SysUser
    {
    }

    // 添加验证信息
    internal class SysUserValidator : AbstractValidator<SysUser>
    {
        public SysUserValidator()
        {
            RuleFor(m => m.LoginName).NotEmpty().WithMessage("Login name is required!".T());
        }
    }
}
