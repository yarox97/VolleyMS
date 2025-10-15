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
        public async Task<Notification> Create(Notification notification)
        {
            var taskEntity = _mapper.Map<NotificationModel>(notification);
            await _context.Notifications.AddAsync(taskEntity);
            await _context.SaveChangesAsync();
            return notification;
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
