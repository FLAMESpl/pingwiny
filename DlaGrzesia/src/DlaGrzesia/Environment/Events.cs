using DlaGrzesia.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DlaGrzesia.Environment
{
    public interface IEvent
    {
    }

    public class Events : ISerializableGameState
    {
        private Dictionary<Type, List<IEvent>> scheduledEvents = new Dictionary<Type, List<IEvent>>();
        private Dictionary<Type, List<IEvent>> currentEvents = new Dictionary<Type, List<IEvent>>();

        public void Add(IEvent @event)
        {
            var type = @event.GetType();
            if (!scheduledEvents.TryGetValue(type, out var list))
            {
                list = new List<IEvent>();
                scheduledEvents.Add(type, list);
            }
            list.Add(@event);
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            var count = stream.ReadInt();
            for (int i = 0; i < count; i++)
                Add((IEvent)serializer.ReadNext(stream));
        }

        public IEnumerable<T> Get<T>() where T : IEvent
        {
            return currentEvents.GetValueOrDefault(typeof(T))?.Cast<T>() ?? Enumerable.Empty<T>();
        }

        public void Progress()
        {
            currentEvents = scheduledEvents;

            if (scheduledEvents.Count != 0)
                scheduledEvents = new Dictionary<Type, List<IEvent>>();
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            var events = this.scheduledEvents.Values.SelectMany(x => x).OfType<ISerializable>().ToList();
            stream.WriteInt(events.Count);
            foreach (var @event in events)
                serializer.WriteNext(stream, @event);
        }

        public bool TryGetFirst<T>(out T @event) where T : IEvent
        {
            @event = Get<T>().FirstOrDefault();
            return @event is not null;
        }
    }
}
