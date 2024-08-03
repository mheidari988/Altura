using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents an field in the AlturaCMS domain.
    /// </summary>
    public class Field : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the attribute in the database table.
        /// </summary>
        /// <value>The name of the attribute in table.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets display name for the field.
        /// </summary>
        /// <value>The display name of the field</value>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field or form field is required.
        /// </summary>
        /// <value><c>true</c> if the field or form field is required; otherwise, <c>false</c>.</value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field or form field is unique.
        /// </summary>
        /// <value><c>true</c> if the field or form field is unique; otherwise, <c>false</c>.</value>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of the field or form field.
        /// </summary>
        /// <value>The minimum length of the field or form field.</value>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the field or form field.
        /// </summary>
        /// <value>The maximum length of the field or form field.</value>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the field or form field.
        /// </summary>
        /// <value>The minimum value of the field or form field.</value>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the field or form field.
        /// </summary>
        /// <value>The maximum value of the field or form field.</value>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum date time of the DateTime FieldType or form field.
        /// </summary>
        /// <value>The minimum date time of the field or form field.</value>
        public DateTime? MinDateTime { get; set; }

        /// <summary>
        /// Gets or sets the maximum date time of the DateTime FieldType or form field.
        /// </summary>
        /// <value>The maximum date time of the DateTime FieldType or form field.</value>
        public DateTime? MaxDateTime { get; set; }

        /// <summary>
        /// Gets or sets the regular expression pattern for the field or form field.
        /// </summary>
        /// <value>The regular expression pattern for the field or form field.</value>
        public string? RegexPattern { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of allowed values for the field or form field.
        /// </summary>
        /// <value>The list of allowed values for the field or form field.</value>
        public List<string>? AllowedValues { get; set; } = [];

        /// <summary>
        /// Gets or sets the reference table name for the field or form field.
        /// </summary>
        /// <value>The reference table name for the field or form field.</value>
        public string? ReferenceTableName { get; set; }

        /// <summary>
        /// Gets or sets a field name from the reference table that we will show in the select or multiselect of the content type.
        /// </summary>
        /// <value>The reference table's chosen field name.</value>
        public string? ReferenceDisplayFieldName { get; set; }
    }
}
