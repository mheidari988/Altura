﻿using AlturaCMS.Domain.Common;

namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents an field in the AlturaCMS domain.
    /// </summary>
    public class Field : BaseEntity
    {
        /// <summary>
        /// Gets or sets the slug of the field.
        /// </summary>
        /// <value>The slug of the field.</value>
        public string Slug { get; set; } = string.Empty;

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
        public List<string>? AllowedValues { get; set; } = [];
    }
}
