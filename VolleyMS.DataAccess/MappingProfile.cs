using AutoMapper;
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

namespace VolleyMS.DataAccess
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<Notification, NotificationEntity>().ReverseMap();
            CreateMap<Club, ClubEntity>().ReverseMap();
            CreateMap<Comment, CommentEntity>().ReverseMap();
            CreateMap<Contract, ContractEntity>().ReverseMap();
            CreateMap<Task, TaskEntity>().ReverseMap();
        }
    }
}
