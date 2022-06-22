namespace Application.PermaNotifications.Queries
{
    public class NotificationDto
    {
        public string Id { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime? SendedAt { get; set; }
        public DateTime? RecivedAt { get; set; }
    }
}
