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
        Text = 1,

        /// <summary>
        /// A rich text field that supports formatting.
        /// </summary>
        RichText = 2,

        /// <summary>
        /// A numeric field.
        /// </summary>
        Number = 3,

        /// <summary>
        /// A field that stores decimal numbers.
        /// </summary>
        Currency = 4,

        /// <summary>
        /// A checkbox field.
        /// </summary>
        Checkbox = 5,

        /// <summary>
        /// A date and time field.
        /// </summary>
        DateTime = 6,

        /// <summary>
        /// A file field for uploading files.
        /// </summary>
        File = 7,

        /// <summary>
        /// A single select field.
        /// </summary>
        Select = 8,

        /// <summary>
        /// A multi-select field that allows multiple selections.
        /// </summary>
        MultiSelect = 9,
    }
}
