using Application.UserNotifications.Queries;
using Domain.Entities.JoinTables;

namespace Infrastructure.Mappers
{
    internal static class Mappers
    {
        internal static IServiceCollection AddCustomMappers(this IServiceCollection services)
        {
            TypeAdapterConfig<UserNotification, NotificationDto>
                .NewConfig()
                .Map(dest => dest.SendedAt, src => src.Notification.CreatedOn)
                .Map(dest => dest.Message, src => src.Notification.Message);

            return services;
        }

    }
}
