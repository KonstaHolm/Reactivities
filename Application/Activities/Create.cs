using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
   public class Create
   {
      public class Command : IRequest<Result<Unit>>
      {
         public Activity Activity { get; set; }
      }

      public class CommandValidator : AbstractValidator<Command>
      {
         public CommandValidator()
         {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
         }
      }

      public class Handler : IRequestHandler<Command, Result<Unit>>
      {
         private readonly DataContext _oContext;
         public Handler(DataContext oContext)
         {
            _oContext = oContext;

         }

         public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
         {
            _oContext.Activities.Add(request.Activity);

            var result = await _oContext.SaveChangesAsync() > 0;

            if(!result) return Result<Unit>.Failure("Failed to create activity");

            return Result<Unit>.Success(Unit.Value);
         }
      }
   }
}