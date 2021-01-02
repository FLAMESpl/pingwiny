using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DlaGrzesia.Objects
{
    public class ObjectsCollection : GameObject, IEnumerable<GameObject>, ISerializable
    {
        private readonly DoubleBuffered<List<GameObject>> objects = DoubleBuffered.Create<List<GameObject>>();

        public int Count => objects.Current.Count;

        public void Add(GameObject @object)
        {
            objects.Current.Add(@object);
            if (Initialized)
                @object.Initialize(Environment, GameState);
        }

        public void Add(IEnumerable<GameObject> objects)
        {
            foreach (var @object in objects)
                Add(@object);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            foreach (var @object in objects.Current)
                @object.Draw(gameTime, batch);
        }

        public override void Update(GameTime gameTime)
        {
            if (Environment?.IsPaused == false)
            {
                foreach (var @object in objects.Current)
                {
                    @object.Update(gameTime);
                    if (@object.IsAlive)
                        objects.Other.Add(@object);
                }

                objects.Current.Clear();
                objects.Swap();
            }
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            var count = stream.ReadInt();
            for (int i = 0; i < count; i++)
                Add((GameObject)serializer.ReadNext(stream));

            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(Count);
            foreach (var @object in objects.Current)
                serializer.WriteNext(stream, @object);

            base.Serialize(stream, serializer);
        }

        protected override void OnInitialized()
        {
            foreach (var @object in objects.Current)
                @object.Initialize(Environment, GameState);
        }

        public IEnumerator<GameObject> GetEnumerator() => objects.Current.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
