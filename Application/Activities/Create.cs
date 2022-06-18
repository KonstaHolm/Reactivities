using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
   public class Create
   {
      public class Command : IRequest
      {
         public Activity Activity { get; set; }
      }

      public class Handler : IRequestHandler<Command>
      {
         private readonly DataContext _oContext;
         public Handler(DataContext oContext)
         {
            _oContext = oContext;

         }

         public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
         {
            _oContext.Activities.Add(request.Activity);

            await _oContext.SaveChangesAsync();

            return Unit.Value;
         }
      }
   }
}