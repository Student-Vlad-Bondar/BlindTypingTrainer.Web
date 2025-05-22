namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Helpers
{
    public static class HttpContextHelper
    {
        public static IServiceProvider RequestServices => new HttpContextAccessor().HttpContext!.RequestServices;
    }
}
