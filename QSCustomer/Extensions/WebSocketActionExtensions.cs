using Microsoft.AspNetCore.SignalR;
using QSCustomer.Hubs;
using QSCustomer.IMainRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QSCustomer.Extensions
{
    public class WebSocketActionExtensions
    {
        protected IHubContext<HomeHub> _context;
        private readonly IUnitOfWork _uow;
        public WebSocketActionExtensions(IHubContext<HomeHub> context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }
        public async Task ProgressBar_WebSocket(Claim Claims, double Count , double Length, string id)
        {
            HomeHub Hub = new HomeHub(_context);
            await Hub.Progressbar(Count,Length,id);
        }
    }
}
