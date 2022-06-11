using Domain.Entities.JoinTables;
using Infrastructure.Identity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.HasOne(un => (ApplicationUser)un.DestinationUser)
                .WithMany(u => u.UserNotifications)
                .HasForeignKey(u => u.DestinationUserId);

            builder.HasOne(un => un.Notification)
                .WithMany(n => n.UserNotifications)
                .HasForeignKey(un => un.NotificationId);

            builder.HasOne(un => (ApplicationUser)un.OriginUser)
                .WithMany()
                .HasForeignKey(un => un.OriginUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
