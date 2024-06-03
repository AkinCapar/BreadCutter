using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BreadCutter.Installers
{
    [CreateAssetMenu(fileName = nameof(GameSettingsInstaller), menuName = Constants.MenuNames.INSTALLERS + nameof(GameSettingsInstaller))]

    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private ScriptableObject[] settings;

        public override void InstallBindings()
        {
            foreach (ScriptableObject setting in settings)
            {
                Container.BindInterfacesAndSelfTo(setting.GetType()).FromInstance(setting);
            }
        }
    }
}
