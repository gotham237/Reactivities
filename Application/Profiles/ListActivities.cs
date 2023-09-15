using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Predicate { get; set; }
            //past event, future event or hosting event
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query =  _context.ActivityAttendees
                    .Where(u => u.AppUser.UserName == request.Username)
                    .OrderBy(d => d.Activity.Date)
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();
                
                query = request.Predicate switch 
                {
                    "past" => query.Where(a => a.Date <= DateTime.UtcNow),
                    "hosting" => query.Where(a => a.HostUsername == request.Username),
                    //hosted events
                    _ => query.Where(a => a.Date > DateTime.UtcNow)
                };

                var activities = await query.ToListAsync();
                return Result<List<UserActivityDto>>.Success(activities);
            }
        }
    }
}