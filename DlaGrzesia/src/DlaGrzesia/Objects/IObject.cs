using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects
{
    public interface IObject
    {
        bool Expired { get; }
        void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers);
        void Update(GameTime elapsed, EnvironmentState environmentState);
    }
}
