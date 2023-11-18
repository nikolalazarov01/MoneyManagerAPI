using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(u => u.Accounts)
            .WithOne(a => a.User);
            builder.HasOne(u => u.BaseCurrency)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
