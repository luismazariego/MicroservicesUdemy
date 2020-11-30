namespace Ordering.Application.Queries
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    using Responses;

    public class GetOrderByUserQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; set; }

        public GetOrderByUserQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
