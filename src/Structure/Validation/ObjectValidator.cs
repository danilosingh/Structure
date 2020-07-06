namespace Structure.Validation
{
    public abstract class ObjectValidator<T> : IObjectValidator<T>
    {
        public ValidatorResult Validate(T validatingObject)
        {
            var result = new ValidatorResult();
            AddErrors(result.Errors, validatingObject);
            return result;
        }

        protected abstract void AddErrors(ValidationErrorCollection errors, T validatingObject);
    }
}
