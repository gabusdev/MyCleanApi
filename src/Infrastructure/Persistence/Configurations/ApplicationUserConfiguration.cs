using Infrastructure.Identity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiuration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //builder.Ignore(e => e.DomainEvents);
        /*
        builder.HasMany(u => u.UserNotifications)
            .WithOne(un => (ApplicationUser)un.DestinationUser)
            .HasForeignKey(un => un.DestinationUserId);
        */
    }
}
