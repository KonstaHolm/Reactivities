using System;
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
   public class Details
   {
      public class Query : IRequest<Result<ActivityDto>>
      {
         public Guid Id { get; set; }
      }

      public class Handler : IRequestHandler<Query, Result<ActivityDto>>
      {
         private DataContext _context;
         private readonly IMapper mapper;
         private readonly IUserAccessor userAccessor;

         public Handler(DataContext oContext, IMapper mapper, IUserAccessor userAccessor)
         {
            _context = oContext;
            this.mapper = mapper;
            this.userAccessor = userAccessor;
         }

         public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
         {
            var activity = await _context.Activities
            .ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUsername = userAccessor.GetUsername() })
            .FirstOrDefaultAsync(x => x.Id == request.Id);

            return Result<ActivityDto>.Success(activity);
         }
      }
   }
}