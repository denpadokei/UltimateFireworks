using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateFireworks.Models;

namespace UltimateFireworks.Installer
{
    public class AppInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<SoundLoader>().AsSingle();
        }
    }
}
