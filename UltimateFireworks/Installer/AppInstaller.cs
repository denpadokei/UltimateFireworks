using UltimateFireworks.Models;
using Zenject;

namespace UltimateFireworks.Installer
{
    public class AppInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<SoundLoader>().FromNewComponentOnNewGameObject().AsCached();
        }
    }
}
