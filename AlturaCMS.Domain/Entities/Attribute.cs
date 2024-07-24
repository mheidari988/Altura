using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents an attribute in the AlturaCMS domain.
    /// </summary>
    public class Attribute : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <value>The name of the attribute.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        /// <value>The type of the attribute.</value>
        public AttributeType Type { get; set; }

        /// <summary>
        /// Gets or sets the validation rules for the attribute.
        /// </summary>
        /// <value>The validation rules for the attribute.</value>
        public ValidationRules ValidationRules { get; set; } = new();
    }
}
