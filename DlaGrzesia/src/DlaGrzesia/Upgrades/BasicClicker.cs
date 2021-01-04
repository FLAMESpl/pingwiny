﻿using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class BasicClicker : Upgrade
    {
        private const int BASE_COOLDOWN = 100;
        private const float COOLDOWN_REDUCTION_RATE = 0.05f;
        private Counter activationCooldown = Counter.NewStarted(BASE_COOLDOWN).ToCyclic();

        public BasicClicker() : base(1)
        {
        }

        public override IUpgradeAction GetAction()
        {
            activationCooldown.Tick();
            return activationCooldown.Elapsed
                ? ClickRandomNonSurfingPenguin.Instance
                : NoOperation.Instance;
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            activationCooldown = stream.ReadStruct<Counter>();
            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteStruct(activationCooldown);
            base.Serialize(stream, serializer);
        }

        protected override void OnLeveledUp()
        {
            activationCooldown.StartFrom(GetCurrentCooldown());
        }

        private int GetCurrentCooldown()
        {
            var reducedCooldown = BASE_COOLDOWN * Math.Pow(1 - COOLDOWN_REDUCTION_RATE, Level - 1);
            return reducedCooldown > 1 ? (int)Math.Round(reducedCooldown) : 1;
        }
    }
}