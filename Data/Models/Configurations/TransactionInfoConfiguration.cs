using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configurations
{
    public class TransactionInfoConfiguration : IEntityTypeConfiguration<TransactionInfo>
    {
        public void Configure(EntityTypeBuilder<TransactionInfo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Total).HasColumnName("Total");
            builder.Property(x => x.Date).HasColumnName("Date");

            builder.HasOne(ai => ai.Account)
            .WithMany(a => a.TransactionInfos)
            .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
