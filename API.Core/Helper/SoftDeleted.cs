using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Helper
{
   public interface ISoftDeleted
    {
         bool Deleted { get; set; }
    }

    public interface IHidden
    {
        bool Hidden { get; set; }
    }
}
