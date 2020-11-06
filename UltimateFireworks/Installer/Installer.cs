using SiraUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace UltimateFireworks.Installer
{
    public class Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UltimateFireworksController>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
