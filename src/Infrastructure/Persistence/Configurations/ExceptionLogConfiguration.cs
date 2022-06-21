using Application.Common.Exceptions.Exception_Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    internal class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
    {
        public void Configure(EntityTypeBuilder<ExceptionLog> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(36);
            builder.Property(e => e.UserId)
                .HasMaxLength(36);
            builder.Property(e => e.IPInfo)
                .HasMaxLength(20);
            builder.Property(e => e.Reference)
                .HasMaxLength(250);
            builder.Property(e => e.Source)
                .HasMaxLength(250);
            builder.Property(e => e.Messages)
                .HasMaxLength(500);
        }
    }
}
