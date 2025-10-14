using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;

namespace VolleyMS.Core.Entities
{
    public class Comment : BaseEntity
    {
        public Guid Id { get; }
        public string Text { get; } = string.Empty;
    }
}
