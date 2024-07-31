using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents a field in a form in the AlturaCMS domain.
    /// </summary>
    public class FormField : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the form field.
        /// </summary>
        /// <value>The name of the form field.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the form field.
        /// </summary>
        /// <value>The type of the form field.</value>
        public FieldType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field or form field is required.
        /// </summary>
        /// <value><c>true</c> if the field or form field is required; otherwise, <c>false</c>.</value>
        public bool IsRequired { get; set; }

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
        public int? MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the field or form field.
        /// </summary>
        /// <value>The maximum value of the field or form field.</value>
        public int? MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the regular expression pattern for the field or form field.
        /// </summary>
        /// <value>The regular expression pattern for the field or form field.</value>
        public string? RegexPattern { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of allowed values for the field or form field.
        /// </summary>
        /// <value>The list of allowed values for the field or form field.</value>
        public List<string> AllowedValues { get; set; } = [];

        /// <summary>
        /// Gets or sets the ID of the form.
        /// </summary>
        /// <value>The ID of the form.</value>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the form associated with this form field.
        /// </summary>
        /// <value>The form associated with this form field.</value>
        public Form Form { get; set; } = new();
    }
}
