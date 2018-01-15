using FluentValidation.Attributes;
using FluentValidation.Mvc;
using Smart.Core.Infrastructure;
using System.Web.Mvc;

namespace Smart.Web.Mvc
{
    class StartupTask : IStartupTask
    {
        public int Order { get { return 0; } }

        public void Execute()
        {
            // 设置 FluentValidation 作为默认的验证提供程序
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new AttributedValidatorFactory()));
        }
    }
}
