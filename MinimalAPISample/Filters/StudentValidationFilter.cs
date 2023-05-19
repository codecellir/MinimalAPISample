using MinimalAPISample.Entities;

namespace MinimalAPISample.Filters
{
    public class StudentValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var student = context.GetArgument<Student>(0);
            if (string.IsNullOrWhiteSpace(student.Name))
            {
                return await Task.FromResult(Results.BadRequest("name is required"));
            }
            return await next(context);
        }
    }
}
