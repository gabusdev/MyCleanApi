using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<PermaNotification>
    {
        public void Configure(EntityTypeBuilder<PermaNotification> builder)
        {
            builder.ToTable("Notification");
            builder.Ignore(n => n.DomainEvents);
            /*
            builder.HasMany(n => n.UserNotifications)
                .WithOne(un => un.Notification)
                .HasForeignKey(un => un.NotificationId);
            */
        }
    }
}
