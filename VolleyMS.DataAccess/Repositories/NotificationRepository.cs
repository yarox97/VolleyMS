using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Entities;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;
using VolleyMS.DataAccess.Models;
using Task = System.Threading.Tasks.Task;


namespace VolleyMS.DataAccess.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly VolleyMsDbContext _context;
        private readonly IMapper _mapper;
        public NotificationRepository(VolleyMsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Notification> Create(Notification notification, IList<Guid> receivers, Guid senderId)
        {
            var senderEntity = await _context.Users.FindAsync(senderId) ?? throw new Exception("Sender not found!");

            var receiversEntities = await _context.Users
               .Where(u => receivers.Contains(u.Id))
               .ToListAsync();

            if (receiversEntities.Count < 1) throw new Exception("No recievers found!");

            var NotificationModel = new NotificationEntity 
            {
                notificationType = notification.NotificationType,
                Text = notification.Text,
                isChecked = notification.IsChecked,
                LinkedURL = notification.LinkedURL,
                senderId = senderId,
                Sender = senderEntity,
                Receivers = receiversEntities,
                CreatorId = senderId
            };

            await _context.Notifications.AddAsync(NotificationModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                throw;
            }
            return _mapper.Map<Notification>(NotificationModel);
        }

        public async Task Delete(Guid NorificationId)
        {
            await _context.Notifications
                .Where(n => n.Id == NorificationId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
        }

        public async Task<IList<Notification>> GetUserNotifications(Guid userId)
        {
            var notifEntities = await _context.Notifications
                .Where(n => n.Receivers.Any(r => r.Id == userId))
                .ToListAsync();

            return  _mapper.Map<IList<Notification>>(notifEntities);
        }

        public async Task Check(Guid NotificationId)
        {
            var notif = await _context.Notifications.FindAsync(NotificationId);
            if (notif == null) return;
            
            notif.isChecked = true;
            await _context.SaveChangesAsync();
        }

    }
}
