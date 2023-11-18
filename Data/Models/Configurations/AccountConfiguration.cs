using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Total).HasColumnName("Total");

            builder.HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Currency)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.TransactionInfos)
            .WithOne(ai => ai.Account);
        }
    }
}
