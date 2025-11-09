using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    internal class User_ClubConfiguration : IEntityTypeConfiguration<User_ClubsEntity>
    {
        public void Configure(EntityTypeBuilder<User_ClubsEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserClubs)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Club)
                .WithMany(c => c.UserClubs)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
