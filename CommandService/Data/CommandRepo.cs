using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;
        private readonly IPlatformRepo _platRepo;

        public CommandRepo(AppDbContext context, IPlatformRepo platformRepo)
        {
            _context = context;

            _platRepo = platformRepo;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            

           if(!_platRepo.PlatformExists(platformId))
                throw new ArgumentException($"No platform with {platformId} exists.");
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .Where(x => x.PlatformId == platformId && x.Id == commandId)
                .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(x => x.PlatformId == platformId).OrderBy(x=>x.Platform.Name).ToList();
        }

        public bool SaveChanges()
        {
           return _context.SaveChanges() >= 0;
        }
    }
}
