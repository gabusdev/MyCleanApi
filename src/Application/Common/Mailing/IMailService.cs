namespace Application.Common.Mailing;

public interface IMailService
{
    /// <summary>
    /// Attemps to Send the Email given by the <c>MailRequest</c>
    /// </summary>
    /// <param name="request">The mail request</param>
    Task SendAsync(MailRequest request);
}