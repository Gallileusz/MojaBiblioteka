namespace MojaBiblioteka.Utility
{
    public static class UserSession
    {
        public static int UserId { get; private set; }
        public static string Login { get; private set; }
        public static bool IsAuthenticated => UserId > 0 && !string.IsNullOrWhiteSpace(Login);

        public static void SignIn(UserModel user)
        {
            UserId = user?.Id ?? 0;
            Login = user?.Login;
        }

        public static void SignOut()
        {
            UserId = 0;
            Login = null;
        }
    }
}
