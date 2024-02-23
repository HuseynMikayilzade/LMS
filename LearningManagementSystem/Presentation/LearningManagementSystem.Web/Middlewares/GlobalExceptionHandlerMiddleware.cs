namespace LearningManagementSystem.Web.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                string errorurl = $"/Error/Errorpage?error={e.Message}";
                context.Response.Redirect(errorurl);
            }
        }
    }
}
