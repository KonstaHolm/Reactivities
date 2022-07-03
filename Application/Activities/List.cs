using System.Collections.Generic;
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
      public class Query : IRequest<Result<List<ActivityDto>>>
      {

      }

      public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
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

         public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
         {
            var activities = await _oContext.Activities
            .ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUsername = userAccessor.GetUsername() })
            .ToListAsync(cancellationToken);

            return Result<List<ActivityDto>>.Success(activities);
         }
      }
   }
}