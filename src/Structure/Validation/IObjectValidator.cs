namespace Structure.Validation
{
    public interface IObjectValidator<T>
    {
        ValidatorResult Validate(T validatingObject);
    }
}
