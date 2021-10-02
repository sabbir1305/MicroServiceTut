using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _context.Platforms.OrderBy(x => x.Name).ToList();
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(x => x.Id == platformId);
        }

        public bool SaveChanges()
        {
           return _context.SaveChanges()>=0;
        }
    }
}
