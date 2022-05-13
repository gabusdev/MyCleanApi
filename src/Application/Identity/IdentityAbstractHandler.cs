using Application.Common.Messaging;
using Application.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity
{
    public abstract class IdentityCommandHandler<TCommand>
        : IdentityCommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    {
        protected IdentityCommandHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
        {
        }
    }
    public abstract class IdentityCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
         where TCommand : ICommand<TResponse>
    {
        protected readonly IUserService _userService;
        protected readonly IHttpContextService _httpContextService;
        public IdentityCommandHandler(IUserService userService, IHttpContextService httpContextService)
        {
            _userService = userService;
            _httpContextService = httpContextService;
        }
        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class IdentityQueryHandler<TQuery>
        : IdentityCommandHandler<TQuery, Unit>
        where TQuery : ICommand<Unit>
    {
        protected IdentityQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
        {
        }
    }
    public abstract class IdentityQueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        protected readonly IUserService _userService;
        protected readonly IHttpContextService _httpContextService;
        public IdentityQueryHandler(IUserService userService, IHttpContextService httpContextService)
        {
            _userService = userService;
            _httpContextService = httpContextService;
        }
        public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
