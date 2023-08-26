using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>> { }
        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            // cancellationToken allows us to cancel request if it is no longer needed
            public async Task<Result<List<ActivityDto>>> Handle(Query request,
                 CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                    // .Include(a => a.Attendees)
                    // .ThenInclude(u => u.AppUser)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                //using Auto mapper
                //var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);

                //return Result<List<Activity>>.Success(await _context.Activities);
                //return Result<List<ActivityDto>>.Success(activitiesToReturn);
                return Result<List<ActivityDto>>.Success(activities);

            }
        }
    }
}