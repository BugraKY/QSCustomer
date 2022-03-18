using Abp.Dependency;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.SignalR;
using QSCustomer.Models.ViewModels;
using QSCustomer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Hubs
{
    public class HomeHub : Hub, ITransientDependency
    {
        protected IHubContext<HomeHub> _context;
        public IAbpSession _abpSession { get; set; }
        public HomeHub(IHubContext<HomeHub> context)
        {
            _abpSession = NullAbpSession.Instance;
            _context = context;
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task Progressbar(double CountProgress, double LengthProgress, string id, string title)
        {
            double rate = 1;

            if (LengthProgress > 0 && CountProgress > 0)
                rate = (CountProgress / LengthProgress) * 100;
            var progressBar = new ProgressBar()
            {
                Id = id,
                Title=title,
                Rate = (int)rate
            };
            await _context.Clients.All.SendAsync("ReceiveProgressRate", progressBar);
        }
        public async Task TriggerSchedulerChange(string id)
        {
            await _context.Clients.All.SendAsync("SchedulerQuery", id);
        }
    }
}
