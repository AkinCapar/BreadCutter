using System.Collections;
using System.Collections.Generic;
using BreadCutter.Models;
using BreadCutter.Views;
using UnityEngine;

namespace BreadCutter.Controllers
{
    public class BreadController : BaseController
    {
        #region Injection

        private BreadModel _breadModel;

        public BreadController(BreadModel breadModel)
        {
            _breadModel = breadModel;
        }

        #endregion

        public override void Initialize()
        {
            _signalBus.Subscribe<BladeSwitchedDirectionSignal>(OnBladeSwitchedDirectionSignal);
        }

        private void OnBladeSwitchedDirectionSignal()
        {
            foreach (List<BreadView> breadLine in _breadModel.breadLines)
            {
                foreach (BreadView bread in breadLine)
                {
                    bread.MoveForward();
                }
            }
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<BladeSwitchedDirectionSignal>(OnBladeSwitchedDirectionSignal);
        }
    }
}