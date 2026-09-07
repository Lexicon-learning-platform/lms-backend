namespace Lms_backend.Domain.Constants
{
    public static class Roles
    {
        public const string Student = "Student";
        public const string Teacher = "Teacher";
        public const string Admin = "Admin";
        public const string TeacherAndAbove = "Teacher,Admin";
        public static readonly string[] All = [Student, Teacher, Admin];
    }
}
