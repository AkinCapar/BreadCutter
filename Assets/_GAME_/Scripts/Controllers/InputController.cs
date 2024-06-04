using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BreadCutter.Controllers
{
    public class InputController : BaseController, ITickable
    {
        private float _timeHeldDown;
        private bool _canGiveInput;
        
        #region Injection

        private LevelSettings _levelSettings;
        
        public InputController(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;
        }
        #endregion
        public override void Initialize()
        {
            _canGiveInput = true;
        }
        
        public void Tick()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && _canGiveInput)
            {
                _timeHeldDown += Time.deltaTime;

                if (_timeHeldDown >= _levelSettings.MaxCooldownAmount)
                {
                    _canGiveInput = false;
                }
            }

            else
            {
                if (_timeHeldDown > 0)
                {
                    _timeHeldDown -= Time.deltaTime;
                    if (_timeHeldDown < 0)
                    {
                        _timeHeldDown = 0;
                        _canGiveInput = true;
                    }
                }
            }
            
            _signalBus.Fire(new PlayerCooldownSignal(_timeHeldDown));
        }

        public override void Dispose()
        {

        }
    }
}
