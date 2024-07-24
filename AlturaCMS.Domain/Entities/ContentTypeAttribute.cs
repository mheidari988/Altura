namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a content type and its attributes in the AlturaCMS domain.
    /// </summary>
    public class ContentTypeAttribute
    {
        /// <summary>
        /// Gets or sets the ID of the content type.
        /// </summary>
        /// <value>The ID of the content type.</value>
        public Guid ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>The content type associated with this attribute.</value>
        public ContentType ContentType { get; set; } = new();

        /// <summary>
        /// Gets or sets the ID of the attribute.
        /// </summary>
        /// <value>The ID of the attribute.</value>
        public Guid AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute associated with this content type.</value>
        public Attribute Attribute { get; set; } = new();
    }
}
