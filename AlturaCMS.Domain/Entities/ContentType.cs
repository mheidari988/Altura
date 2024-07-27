﻿using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents a content type in the AlturaCMS domain.
    /// </summary>
    public class ContentType : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the content type.
        /// </summary>
        /// <value>The name of the content type.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of fields for the content type.
        /// </summary>
        /// <value>The collection of fields for the content type.</value>
        public ICollection<ContentTypeField> Fields { get; set; } = [];
    }
}
