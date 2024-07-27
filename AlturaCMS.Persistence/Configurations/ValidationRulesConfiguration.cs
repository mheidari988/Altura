using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlturaCMS.Persistence.Configurations
{
    public class ValidationRulesConfiguration : BaseEntityConfiguration<ValidationRules>
    {
        public override void Configure(EntityTypeBuilder<ValidationRules> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.RegexPattern)
                .HasMaxLength(200);

            builder.Property(e => e.AllowedValues)
                .HasConversion(new StringListToStringConverter())
                .Metadata.SetValueComparer(new StringListValueComparer());
        }

        /// <summary>
        /// Converts a <see cref="List{T}"/> of <see cref="string"/> to a <see cref="string"/> and vice versa.
        /// </summary>
        internal class StringListToStringConverter : ValueConverter<List<string>, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StringListToStringConverter"/> class.
            /// </summary>
            public StringListToStringConverter()
                : base(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            {
            }
        }

        /// <summary>
        /// Compares instances of <see cref="List{T}"/> of <see cref="string"/>.
        /// </summary>
        internal class StringListValueComparer : ValueComparer<List<string>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StringListValueComparer"/> class.
            /// </summary>
            public StringListValueComparer()
                : base(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList())
            {
            }
        }
    }
}
