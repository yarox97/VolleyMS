using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<ClubEntity>
    {
        public void Configure(EntityTypeBuilder<ClubEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
