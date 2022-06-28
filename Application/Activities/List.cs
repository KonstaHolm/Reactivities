using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
   public class List
   {
      public class Query : IRequest<Result<List<Activity>>>
      {

      }

      public class Handler : IRequestHandler<Query, Result<List<Activity>>>
      {
         private readonly DataContext _oContext;
         public Handler(DataContext context)
         {
            _oContext = context;
         }

         public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
         {
            return Result<List<Activity>>.Success(await _oContext.Activities.ToListAsync());
         }
      }
   }
}