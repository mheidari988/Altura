using AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;
using AlturaCMS.Domain;
using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application
{
    public static class ApplicationShared
    {
        /// <summary>
        /// Retrieves a list of default fields used in the content type creation process.
        /// These fields include metadata such as Slug, CreatedDate, CreatedBy, UpdatedDate,
        /// UpdatedBy, IsDeleted, DeletedDate, DeletedBy, RowVersion, and IsActive.
        /// </summary>
        /// <returns>A list of <see cref="CreateContentTypeFieldDto"/> representing the default fields.</returns>
        public static List<CreateContentTypeFieldDto> GetDefaultFields()
        {
            List<CreateContentTypeFieldDto> fields =
            [
                new() {
                    Name = DomainShared.Constants.DefaultFields.Slug.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.Slug.DisplayName,
                    FieldType = FieldType.Text,
                    IsRequired = true,
                    IsUnique = true,
                    MaxLength = DomainShared.Constants.DefaultFields.Slug.MaxLength,
                    RegexPattern = null
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.CreatedDate.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.CreatedDate.DisplayName,
                    FieldType = FieldType.DateTime,
                    IsRequired = true
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.CreatedBy.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.CreatedBy.DisplayName,
                    FieldType = FieldType.Text,
                    IsRequired = true,
                    MaxLength = DomainShared.Constants.DefaultFields.CreatedBy.MaxLength,
                    RegexPattern = null
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.UpdatedDate.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.UpdatedDate.DisplayName,
                    FieldType = FieldType.DateTime
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.UpdatedBy.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.UpdatedBy.DisplayName,
                    FieldType = FieldType.Text,
                    MaxLength = DomainShared.Constants.DefaultFields.UpdatedBy.MaxLength,
                    RegexPattern = null
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.IsDeleted.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.IsDeleted.DisplayName,
                    FieldType = FieldType.Checkbox,
                    IsRequired = true
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.DeletedDate.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.DeletedDate.DisplayName,
                    FieldType = FieldType.DateTime
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.DeletedBy.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.DeletedBy.DisplayName,
                    FieldType = FieldType.Text,
                    MaxLength = DomainShared.Constants.DefaultFields.DeletedBy.MaxLength,
                    RegexPattern = null
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.RowVersion.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.RowVersion.DisplayName,
                    FieldType = FieldType.Text,
                    IsRequired = true
                },
                new() {
                    Name = DomainShared.Constants.DefaultFields.IsActive.Name,
                    DisplayName = DomainShared.Constants.DefaultFields.IsActive.DisplayName,
                    FieldType = FieldType.Checkbox,
                    IsRequired = true
                }
            ];

            return fields;
        }
    }
}
