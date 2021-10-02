using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
   public interface IBaseDbRepo
    {
        bool SaveChanges();
    }
}
