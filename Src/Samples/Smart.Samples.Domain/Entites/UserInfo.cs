using FluentValidation.Attributes;

namespace Smart.Samples.Domain.Entites
{
    [Validator(typeof(Validators.UserInfoValidator))]
    public class UserInfo 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
