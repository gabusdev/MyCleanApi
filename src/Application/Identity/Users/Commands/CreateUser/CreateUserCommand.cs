using Application.Common.Events;

namespace Application.Identity.Users.Commands.CreateUser;

public class CreateUserCommand : ICommand<string>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string? PhoneNumber { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, string>
{
    private readonly IUserService _userService;
    private readonly IHttpContextService _httpContextService;
    private readonly IDomainEventService _eventService;
    private readonly IJobService _jobService;

    public CreateUserCommandHandler(IUserService userService, IHttpContextService httpContextService,
        IDomainEventService eventService, IJobService jobService)
    {
        _userService = userService;
        _httpContextService = httpContextService;
        _eventService = eventService;
        _jobService = jobService;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUserId = await _userService.CreateAsync(request, _httpContextService.GetOrigin());
        // Launches the UserCreatedEvent
        //_jobService.Enqueue(() => _eventService.Publish(new UserCreatedEvent(newUserId)));
        //await _eventService.Publish(new UserCreatedEvent(newUserId));
        return newUserId;
    }
}