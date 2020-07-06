using Castle.DynamicProxy;

namespace Structure.Validation.Interception
{
    public class ValidationInterceptor : IInterceptor
    {
        private readonly IMethodInvocationValidator validator;

        public ValidationInterceptor(IMethodInvocationValidator validator)
        {
            this.validator = validator;
        }

        public void Intercept(IInvocation invocation)
        {
            validator.Validate(invocation.MethodInvocationTarget, invocation.Arguments);            
            invocation.Proceed();
        }
    }
}
