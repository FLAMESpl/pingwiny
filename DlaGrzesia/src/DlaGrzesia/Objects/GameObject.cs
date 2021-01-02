using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace DlaGrzesia.Objects
{
    public abstract class GameObject : ISerializable
    {
        public bool IsAlive { get; private set; } = true;

        protected bool Initialized { get; private set; }
        protected GameEnvironment Environment { get; private set; }
        protected GameState GameState { get; private set; }

        public void Initialize(GameEnvironment environment, GameState gameState)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            GameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            OnInitialized();
            Initialized = true;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        protected void Destroy()
        {
            IsAlive = false;
        }

        protected void Publish(IEvent @event)
        {
            GameState.Events.Add(@event);
        }

        protected void Schedule(ICommand command)
        {
            Environment.Commands.Add(command);
        }

        protected virtual void OnInitialized()
        {
        }

        public virtual void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteBool(IsAlive);
        }

        public virtual void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            IsAlive = stream.ReadBool();
        }
    }
}
