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
        public AttributeType Type { get; set; }

        /// <summary>
        /// Gets or sets the validation rules for the form field.
        /// </summary>
        /// <value>The validation rules for the form field.</value>
        public ValidationRules ValidationRules { get; set; } = new();

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
