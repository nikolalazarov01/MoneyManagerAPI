using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configurations
{
    public class CurrencyInfoConfiguration : IEntityTypeConfiguration<CurrencyInfo>
    {
        public void Configure(EntityTypeBuilder<CurrencyInfo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BuyRate).HasColumnName("BuyRate");
            builder.Property(x => x.SellRate).HasColumnName("SellRate");
            builder.Property(x => x.Date).HasColumnName("Date");

            builder.HasOne(ci => ci.Currency)
            .WithMany(c => c.CurrencyInfos)
            .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
