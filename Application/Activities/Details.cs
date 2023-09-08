using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        // "query" because it return activities
        // "command" if there is nothing to return
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            public readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var activity = await _context.Activities.FindAsync(request.Id);
                var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, 
                        new {currentUsername = _userAccessor.GetUsername()})
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                
                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}