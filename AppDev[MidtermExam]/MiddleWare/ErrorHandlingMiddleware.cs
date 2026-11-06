using AppDev_MidtermExam_.Common;

namespace AppDev_MidtermExam_.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) 
        {
        _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500; // Internal Server Error
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "An unexpected error occurred. Please try again later.",
                Data = null
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
