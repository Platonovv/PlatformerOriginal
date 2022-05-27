using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class RefillFuelComponent : MonoBehaviour
    {
        private GameSession _session;

        public void Refill()
        {
            GameSession.Instance.Data.Fuel.Value = 100;
        }

    }
}