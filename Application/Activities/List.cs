using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
   public class List
   {
      public class Query : IRequest<List<Activity>>
      {

      }

      public class Handler : IRequestHandler<Query, List<Activity>>
      {
         private readonly DataContext _oContext;
         public Handler(DataContext context)
         {
            _oContext = context;
         }

         public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
         {
            return await _oContext.Activities.ToListAsync();
         }
      }
   }
}