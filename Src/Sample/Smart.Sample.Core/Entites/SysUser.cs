using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Smart.Core.Extensions;
using System.Collections.Generic;

namespace Smart.Sample.Core.Entites
{
    // 用户实体类扩展，public partial 开头，类名必须和表名一样
    [Validator(typeof(SysUserValidator))]
    public partial class SysUser
    {
        [JsonProperty]
        private Dictionary<string, object> dics;
        public object this[string key]
        {
            get
            {
                if (dics == null) dics = new Dictionary<string, object>();
                object ret = null;
                dics.TryGetValue(key, out ret);
                return ret;
            }
            set
            {
                if (dics == null) dics = new Dictionary<string, object>();
                dics[key] = value;
            }
        }
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
