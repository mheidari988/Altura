namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents the types of attributes that can be used in AlturaCMS.
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// A plain text attribute.
        /// </summary>
        Text,

        /// <summary>
        /// A rich text attribute that supports formatting.
        /// </summary>
        RichText,

        /// <summary>
        /// A numeric attribute.
        /// </summary>
        Number,

        /// <summary>
        /// A checkbox attribute.
        /// </summary>
        Checkbox,

        /// <summary>
        /// A date and time attribute.
        /// </summary>
        DateTime,

        /// <summary>
        /// A file attribute for uploading files.
        /// </summary>
        File,

        /// <summary>
        /// A single select attribute.
        /// </summary>
        Select,

        /// <summary>
        /// A multi-select attribute that allows multiple selections.
        /// </summary>
        MultiSelect
    }
}
