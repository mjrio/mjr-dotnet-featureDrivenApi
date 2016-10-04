using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V1.Tickets
{
    public class AddTicket
    {
        public class Command: IRequest<Result>
        {

        }

        public class Result
        {
            public int Id { get; set; }
        }
    }
}
