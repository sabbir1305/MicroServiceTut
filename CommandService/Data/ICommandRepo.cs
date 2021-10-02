using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public interface ICommandRepo:IBaseDbRepo
    {


        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);

        void CreateCommand(int platformId, Command command);

    }
}
