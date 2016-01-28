using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;
using System.Diagnostics;

namespace Smart.Tests
{
    [TestClass]
    public class FluentValidationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var user = new User { Id = 0, Name = "" };
            var validator = new UserValidator();
            var results = validator.Validate(user);
            var validationSucceeded = results.IsValid;
            var failures = results.Errors;
            failures.ToList().ForEach(t => Debug.WriteLine(t.ErrorMessage));
        }


        class UserValidator : AbstractValidator<User>
        {
            public UserValidator()
            {
                this.RuleFor(u => u.Id).GreaterThan(0).WithMessage("用户Id必须大于0！");
                this.RuleFor(u => u.Name).Length(2, 50).WithMessage("用户姓名长度在2-20之间！");
            }
        }

        class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Birthday { get; set; }
        }
    }
}
