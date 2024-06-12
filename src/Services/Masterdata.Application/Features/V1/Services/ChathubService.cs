using Core.Constant;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Masterdata.Application.Features.V1.Services
{
    public class ChathubService : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Gửi tin nhắn đến tất cả các client đang kết nối
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
