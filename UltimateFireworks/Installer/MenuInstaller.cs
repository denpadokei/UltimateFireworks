using UltimateFireworks.Views;
using Zenject;

namespace UltimateFireworks.Installer
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<Setting>().FromNewComponentAsViewController().AsCached();
            this.Container.BindInterfacesAndSelfTo<UltimateFireworksController>().FromNewComponentOnNewGameObject().AsCached();
        }
    }
}
