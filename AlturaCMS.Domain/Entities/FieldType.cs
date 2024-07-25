namespace AlturaCMS.Domain.Entities
{
    /// <summary>
    /// Represents the types of fields that can be used in AlturaCMS.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// A plain text field.
        /// </summary>
        Text,

        /// <summary>
        /// A rich text field that supports formatting.
        /// </summary>
        RichText,

        /// <summary>
        /// A numeric field.
        /// </summary>
        Number,

        /// <summary>
        /// A checkbox field.
        /// </summary>
        Checkbox,

        /// <summary>
        /// A date and time field.
        /// </summary>
        DateTime,

        /// <summary>
        /// A file field for uploading files.
        /// </summary>
        File,

        /// <summary>
        /// A single select field.
        /// </summary>
        Select,

        /// <summary>
        /// A multi-select field that allows multiple selections.
        /// </summary>
        MultiSelect
    }
}
