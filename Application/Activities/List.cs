using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>> 
        {
            public ActivityParams Params{ get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }
            // cancellationToken allows us to cancel request if it is no longer needed
            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request,
                 CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    // .Include(a => a.Attendees)
                    // .ThenInclude(u => u.AppUser)
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, 
                        new {currentUsername = _userAccessor.GetUsername()})
                    //.ToListAsync(cancellationToken);
                    .AsQueryable();

                if (request.Params.isGoing && !request.Params.isHost) 
                {
                    query = query.Where(x => x.Attendees.Any(a => a.Username == _userAccessor.GetUsername()));
                }

                if (request.Params.isHost && !request.Params.isGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
                }

                //using Auto mapper
                //var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);

                //return Result<List<Activity>>.Success(await _context.Activities);
                //return Result<List<ActivityDto>>.Success(activitiesToReturn);
                return Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber,
                        request.Params.PageSize)
                );

            }
        }
    }
}