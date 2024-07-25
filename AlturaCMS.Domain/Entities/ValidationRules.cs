namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents the validation rules for an field or form field in the AlturaCMS domain.
    /// </summary>
    public class ValidationRules
    {
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
        public string RegexPattern { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of allowed values for the field or form field.
        /// </summary>
        /// <value>The list of allowed values for the field or form field.</value>
        public List<string> AllowedValues { get; set; } = [];
    }
}
