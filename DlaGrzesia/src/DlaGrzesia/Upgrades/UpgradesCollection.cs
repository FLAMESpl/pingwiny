using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DlaGrzesia.Upgrades
{
    public class UpgradesCollection : GameObject, ISerializableGameState
    {
        private const int UPGRADES_COUNT = 8;

        private readonly List<int> tilesetIndices;
        private readonly List<Upgrade> upgrades;
        private IReadOnlyList<Tileset> tilesets;

        public Upgrade this[int index] => upgrades[index];

        public UpgradesCollection()
        {
            var random = new Random();

            tilesetIndices = random.Shuffle(Enumerable.Range(0, 5));

            tilesetIndices.Add(0);
            tilesetIndices.Add(0);
            tilesetIndices.Add(0);

            upgrades = new List<Upgrade>
            {
                new BasicClicker(),
                new SurfingClicker(),
                new SurfingFrequencyUpgrade(),
                new Upgrade(100),
                new Upgrade(250),
                new Upgrade(500),
                new Upgrade(1000),
                new Upgrade(2000)
            };
        }

        public Tileset GetTileset(int upgradeIndex) => tilesets[tilesetIndices[upgradeIndex]];

        public override void Update(GameTime gameTime)
        {
            if (!Environment.IsPaused)
            {
                var actions = upgrades.Where(static u => u.Bought).Select(static u => u.GetAction());
                foreach (var action in actions)
                    action.Execute(GameState);
            }
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            for (int i = 0; i < UPGRADES_COUNT; i++)
            {
                tilesetIndices[i] = stream.ReadInt();
                upgrades[i] = (Upgrade)serializer.ReadNext(stream);
            }
            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            for (int i = 0; i < UPGRADES_COUNT; i++)
            {
                stream.WriteInt(tilesetIndices[i]);
                serializer.WriteNext(stream, upgrades[i]);
            }
            base.Serialize(stream, serializer);
        }

        protected override void OnInitialized()
        {
            var textures = Environment.Resources.Textures;
            tilesets = new[]
            {
                textures.Alex,
                textures.Kamil,
                textures.Marcin,
                textures.Marek,
                textures.Tymon
            };
        }
    }
}
