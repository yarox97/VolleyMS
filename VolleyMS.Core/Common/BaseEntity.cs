using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Models;

namespace VolleyMS.Core.Common
{
    public class BaseEntity : AuditableFields
    {
        public Guid? CreatorId { get; set; }
    }
}
