using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; }
        public string Text { get; } = string.Empty;
    }
}
