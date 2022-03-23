namespace E_LearningSystem.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FirstNameMinLength = 5;
            public const int FirstNameMaxLength = 50;
            public const int LastNameMinLength = 5;
            public const int LastNameMaxLength = 50;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Course
        {
            public const int CourseNameMinLength = 5;
            public const int CourseNameMaxLength = 150;
            public const int CourseDescriptionMinLength = 20;
            public const int CourseDescriptionMaxLength = 1000;
        }

        public class CourseCategory
        {
            public const int CourseCategoryNameMinLength = 5;
            public const int CourseCategoryNameMaxLength = 100;       
        }

        public class Lecture
        {
            public const int LectureNameMinLength = 5;
            public const int LectureNameMaxLength = 150;   
            public const int LectureDescriptionMinLength = 20;
            public const int LectureDescriptionMaxLength = 1000;
        }

        public class Resource
        {
            public const int ResourceNameMinLength = 10;
            public const int ResourceNameMaxLength = 150;
        }

        public class ResourceType
        {
            public const int ResourceTypeNameMinLength = 5;
            public const int ResourceTypeNameMaxLength = 100;
        }

        public class Issue
        {
            public const int IssueTitleMinLength = 10;
            public const int IssueTitleMaxLength = 150;

            public const int IssueDescriptionMinLength = 20;
            public const int IssueDescriptionMaxLength = 1000;
        }

        public class Comment
        {
            public const int CommentContentMinLength = 2;
            public const int CommentContentMaxLength = 300;

            public const int IssueDescriptionMinLength = 20;
            public const int IssueDescriptionMaxLength = 1000;
        }
    }
}
