using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
         private readonly IUserAccessor userAccessor;

         public Handler(DataContext oContext, IUserAccessor userAccessor)
         {
            _oContext = oContext;
            this.userAccessor = userAccessor;
         }

         public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
         {
            var user = await _oContext.Users.FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

            var attendee = new ActivityAttendee
            {
               AppUser = user,
               Activity = request.Activity
            };

            request.Activity.Attendees.Add(attendee);

            _oContext.Activities.Add(request.Activity);

            var result = await _oContext.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to create activity");

            return Result<Unit>.Success(Unit.Value);
         }
      }
   }
}