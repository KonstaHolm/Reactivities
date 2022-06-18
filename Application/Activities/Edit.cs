using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
   public class Edit
   {
      public class Command : IRequest
      {
         public Activity Activity { get; set; }
      }

      public class Handler : IRequestHandler<Command>
      {
         private readonly DataContext _context;
         private readonly IMapper _mapper;

         public Handler(DataContext oContext, IMapper mapper)
         {
            _context = oContext;
            _mapper = mapper;
         }

         public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
         {
            //Get activity object from database with id that is passed to PUT-request and update it with automapper in fields passed inside request's body and then save to db asynchronously
            var activity = await _context.Activities.FindAsync(request.Activity.Id);

            _mapper.Map(request.Activity, activity);

            await _context.SaveChangesAsync();

            return Unit.Value;
         }
      }
   }
}