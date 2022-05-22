using Application.Common.CQRS;
using Application.Common.Pagination;

namespace Application.Identity.Users.UserQueries.GetAllPaged
{
    public class GetAllUsersPagedQuery : IQuery<PagedList<UserDetailsDto>>
    {
        private PaginationParams _paginationParams { get; } = null!;

        public GetAllUsersPagedQuery(PaginationParams paginationParams)
        {
            _paginationParams = paginationParams;
        }
        public class GetAllUsersPagedQueryHandler : IdentityQueryHandler<GetAllUsersPagedQuery, PagedList<UserDetailsDto>>
        {
            public GetAllUsersPagedQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
            {
            }

            public override async Task<PagedList<UserDetailsDto>> Handle(GetAllUsersPagedQuery request, CancellationToken cancellationToken)
            {
                return await _userService.GetPagedListAsync(request._paginationParams, cancellationToken);
            }
        }
    }
}
