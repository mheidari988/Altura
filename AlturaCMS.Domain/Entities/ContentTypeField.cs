﻿namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a content type and its fields in the AlturaCMS domain.
    /// </summary>
    public class ContentTypeField
    {
        /// <summary>
        /// Gets or sets the ID of the content type.
        /// </summary>
        /// <value>The ID of the content type.</value>
        public Guid ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>The content type associated with this field.</value>
        public ContentType ContentType { get; set; } = new();

        /// <summary>
        /// Gets or sets the ID of the field.
        /// </summary>
        /// <value>The ID of the field.</value>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field associated with this content type.</value>
        public Field Attribute { get; set; } = new();
    }
}
