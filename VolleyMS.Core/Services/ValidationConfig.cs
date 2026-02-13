namespace VolleyMS.Core.Services
{
    public static class ValidationConfig
    {
        // User
        public const int MAX_USERNAME_LENGTH = 50;
        public const int MAX_NAME_LENGTH = 50;
        public const int MAX_SURNAME_LENGTH = 50;

        public const int MIN_PASSWORD_LENGTH = 5;
        public const int MIN_USERNAME_LENGHT = 5;

        // Club
        public const int MAX_CLUB_NAME_LENGTH = 100;

        // Task
        public static int MAX_TASK_TITLE = 100;
        public static int MAX_TASK_FILES_ALLOWED = 5;

        // Comment
        public static int MAX_COMMENT_TEXT = 1024;
    }
}
