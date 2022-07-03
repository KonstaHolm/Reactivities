using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
   public class List
   {
      public class Query : IRequest<Result<PagedList<ActivityDto>>>
      {
         public ActivityParams Params { get; set; }
      }

      public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
      {
         private readonly DataContext _oContext;
         private readonly IMapper mapper;
         private readonly IUserAccessor userAccessor;

         public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
         {
            _oContext = context;
            this.mapper = mapper;
            this.userAccessor = userAccessor;
         }

         public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
         {
            var query = _oContext.Activities
            .Where(d => d.Date >= request.Params.StartDate)
            .OrderBy(d => d.Date)
            .ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUsername = userAccessor.GetUsername() })
            .AsQueryable();

            if (request.Params.isGoing && !request.Params.isHost)
            {
               query = query.Where(x => x.Attendees.Any(a => a.Username == userAccessor.GetUsername()));
            }

            if (request.Params.isHost && !request.Params.isGoing)
            {
               query = query.Where(x => x.HostUsername == userAccessor.GetUsername());
            }

            return Result<PagedList<ActivityDto>>.Success(await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize));
         }
      }
   }
}