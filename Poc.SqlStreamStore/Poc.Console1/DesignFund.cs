using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Poc.Console1
{
    public class DesignFund
    {
        public class Command : IRequest<bool>
        {
            public string Isin { get; set; }
            public string Currency { get; set; }
            public string Name { get; set; }
        }

        
    }
}
