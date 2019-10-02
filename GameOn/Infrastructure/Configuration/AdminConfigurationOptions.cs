namespace GameOn.Web.Infrastructure.Configuration
{
    public class AdminConfigurationOptions
    {
        public bool NeedToLogInToSeeResults { get; set; }
        public bool NeedToBeLoggedInToAddResults { get; set; } = true;
        public bool EnableGoogleLogIn { get; set; }
    }
}
