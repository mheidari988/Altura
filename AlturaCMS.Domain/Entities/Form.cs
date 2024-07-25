using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents a form in the AlturaCMS domain.
    /// </summary>
    public class Form : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the form.
        /// </summary>
        /// <value>The name of the form.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of form fields for the form.
        /// </summary>
        /// <value>The collection of fields for the form.</value>
        public ICollection<FormField> FormFields { get; set; } = [];
    }
}
