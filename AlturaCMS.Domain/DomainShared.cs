namespace AlturaCMS.Domain;
public static class DomainShared
{
    public static class Constants
    {
        public const string DynamicSchema = "Dynamic";
        public const string MetadataSchema = "Metadata";
        public static class DefaultFields
        {
            public static class Slug
            {
                public const string Name = "Slug";
                public const string DisplayName = "Slug";
                public const int MaxLength = 500;
            }

            public static class CreatedDate
            {
                public const string Name = "CreatedDate";
                public const string DisplayName = "Created Date";
            }

            public static class CreatedBy
            {
                public const string Name = "CreatedBy";
                public const string DisplayName = "Created By";
                public const int MaxLength = 500;
            }

            public static class UpdatedDate
            {
                public const string Name = "UpdatedDate";
                public const string DisplayName = "Updated Date";
            }

            public static class UpdatedBy
            {
                public const string Name = "UpdatedBy";
                public const string DisplayName = "Updated By";
                public const int MaxLength = 500;
            }

            public static class IsDeleted
            {
                public const string Name = "IsDeleted";
                public const string DisplayName = "Is Deleted";
            }

            public static class DeletedDate
            {
                public const string Name = "DeletedDate";
                public const string DisplayName = "Deleted Date";
            }

            public static class DeletedBy
            {
                public const string Name = "DeletedBy";
                public const string DisplayName = "Deleted By";
                public const int MaxLength = 500;
            }

            public static class IsActive
            {
                public const string Name = "IsActive";
                public const string DisplayName = "Is Active";
            }

            public static class RowVersion
            {
                public const string Name = "RowVersion";
                public const string DisplayName = "Row Version";
            }
        }
    }
}
