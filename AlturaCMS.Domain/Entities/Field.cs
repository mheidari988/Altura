using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents an field in the AlturaCMS domain.
    /// </summary>
    public class Field : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <value>The name of the field.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public FieldType Type { get; set; }

        /// <summary>
        /// Gets or sets the ID of the ValidationRules.
        /// </summary>
        /// <value>The ID of the ValidationRules.</value>
        public Guid ValidationRulesId { get; set; }

        /// <summary>
        /// Gets or sets the validation rules for the field.
        /// </summary>
        /// <value>The validation rules for the field.</value>
        public ValidationRules ValidationRules { get; set; } = new();
    }
}
