using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublishEvent:
                    AddPlatform(message);
                    break;
                case EventType.Undetermined:
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            Console.WriteLine("--> Event name");

            var eventType = GetEventName(message);
            switch (eventType.Event)
            {
                case "Platform_Published":
                    return EventType.PlatformPublishEvent;
                default:
                    break;
            }
            return EventType.Undetermined;
        }


        private GenericEventDto GetEventName(string message)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

            return eventType;
        }
      
        private void AddPlatform(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var commandRepo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformRepo = scope.ServiceProvider.GetRequiredService<IPlatformRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(message);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if (commandRepo.ExternalPlatformExists(platform.ExternalID))
                    {
                        Console.WriteLine("Platform already exists.");
                    }
                    else
                    {
                        platformRepo.CreatePlatform(platform);
                        platformRepo.SaveChanges();
                        Console.WriteLine($"Platform added {platform.Name}");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Failed to save message to platform: {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        PlatformPublishEvent,
        Undetermined
    }
}
