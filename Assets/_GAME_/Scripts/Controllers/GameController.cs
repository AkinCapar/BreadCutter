using System;
using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using BreadCutter.Utils;
using UnityEngine;
using Zenject;

namespace BreadCutter.Controllers
{
    public class GameController : BaseController, IInitializable, ITickable, IDisposable
    {
        private GameStates _gameState = GameStates.WaitingToStartState;

        #region Injection

        private LevelController _levelController;
        private BasketSpawnController _basketSpawnController;
        private BreadSpawnController _breadSpawnController;
        private BladeController _bladeController;
        private BasketController _basketController;
        private BreadController _breadController;
        private ScreenController _screenController;
        private SliceController _sliceController;
        private MergeController _mergeController;
        private InputController _inputController;

        public GameController(LevelController levelController
            , BasketSpawnController basketSpawnController
            , BreadSpawnController breadSpawnController
            , BladeController bladeController
            , BasketController basketController
            , BreadController breadController
            , ScreenController screenController
            , SliceController sliceController
            , MergeController mergeController
            , InputController inputController)
        {
            _levelController = levelController;
            _breadSpawnController = breadSpawnController;
            _basketSpawnController = basketSpawnController;
            _bladeController = bladeController;
            _basketController = basketController;
            _breadController = breadController;
            _screenController = screenController;
            _sliceController = sliceController;
            _mergeController = mergeController;
            _inputController = inputController;
        }

        #endregion

        public override void Initialize()
        {
        }

        public void Tick()
        {
            switch (_gameState)
            {
                case GameStates.WaitingToStartState:
                {
                    UpdateStarting();
                    break;
                }

                case GameStates.Playing:
                {
                    UpdatePlayState();
                    break;
                }
            }
        }

        private void UpdatePlayState()
        {
        }

        private void UpdateStarting()
        {
            if (_gameState != GameStates.WaitingToStartState)
            {
                return;
            }

            StartGame();
        }

        private void StartGame()
        {
            if (_gameState != GameStates.WaitingToStartState)
            {
                return;
            }

            ChangeGameState(GameStates.Playing);
            
            _basketSpawnController.Initialize();
            _breadSpawnController.Initialize();
            _bladeController.Initialize();
            _basketController.Initialize();
            _breadController.Initialize();
            _screenController.Initialize();
            _sliceController.Initialize();
            _mergeController.Initialize();
            _inputController.Initialize();
            _levelController.Initialize();
        }

        private void ChangeGameState(GameStates state)
        {
            _gameState = state;
        }

        public override void Dispose()
        {
            _basketSpawnController.Dispose();
            _breadSpawnController.Dispose();
            _bladeController.Dispose();
            _basketController.Dispose();
            _breadController.Dispose();
            _screenController.Dispose();
            _sliceController.Dispose();
            _mergeController.Dispose();
            _inputController.Dispose();
            _levelController.Dispose();
        }
    }
}
