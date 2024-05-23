using System;
using UnityEngine;

namespace Gameplay.Utility
{
    public class ApplicationStateAnnouncer : MonoBehaviour
    {
        public static event Action<bool> OnApplicationFocusChangedEvent;

        private void OnApplicationFocus(bool hasFocus) => OnApplicationFocusChangedEvent?.Invoke(hasFocus);
    }
}