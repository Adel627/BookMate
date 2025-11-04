using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookMate.web.Data.Configurations
{
    public class RentalBookConfiguration : IEntityTypeConfiguration<RentalBook>
    {
        public void Configure(EntityTypeBuilder<RentalBook> builder)
        {
            builder.HasKey(rb => new { rb.RentalId, rb.BookId });
        }
    }
}
