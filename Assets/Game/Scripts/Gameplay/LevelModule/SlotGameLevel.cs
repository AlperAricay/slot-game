using Gameplay.SlotModule.Controller;
using UnityEngine;

namespace Gameplay.LevelModule
{
    public class SlotGameLevel : MonoBehaviour
    {
        [SerializeField] private SlotMachineController _slotMachineController;
        
        private void Awake()
        {
            _slotMachineController.Initialize();
        }
    }
}
