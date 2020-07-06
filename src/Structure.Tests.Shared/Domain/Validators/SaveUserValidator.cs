using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Entities;
using Structure.Validation;

namespace Structure.Tests.Shared.Domain.Validators
{
    public class SaveUserValidator : ObjectValidator<User>
    {
        private readonly IUserRepository userRepository;

        public SaveUserValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        protected override void AddErrors(ValidationErrorCollection errors, User validatingObject)
        {
            if(!userRepository.UserIsUnique(validatingObject.Id, validatingObject.UserName))
            {
                errors.Add("Usuário já existe", "UserName");
            }
        }
    }
}
