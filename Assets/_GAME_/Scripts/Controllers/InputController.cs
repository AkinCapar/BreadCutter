using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BreadCutter.Controllers
{
    public class InputController : BaseController, ITickable
    {
        public override void Initialize()
        {

        }
        
        public void Tick()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _signalBus.Fire<PlayerTappingScreenSignal>();
            }
        }

        public override void Dispose()
        {

        }
    }
}
