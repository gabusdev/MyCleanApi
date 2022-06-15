using Application.UserNotifications.Queries;
using Domain.Entities.JoinTables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappers
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
