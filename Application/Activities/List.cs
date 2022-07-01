using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
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

         public Handler(DataContext context, IMapper mapper)
         {
            _oContext = context;
            this.mapper = mapper;
         }

         public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
         {
            var activities = await _oContext.Activities
            .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

            return Result<List<ActivityDto>>.Success(activities);
         }
      }
   }
}