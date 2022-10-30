using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
   public class EventModeName: BindableBase
    {
        public EventModeName(EventMode mode,string name)
        {
              Mode = mode;
             Name = name;
        }
        public EventMode Mode { get; set; }
        public string? Name { get; set; }
    }
}
