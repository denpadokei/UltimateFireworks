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
