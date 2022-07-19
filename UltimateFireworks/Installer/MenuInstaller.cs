using Zenject;

namespace UltimateFireworks.Installer
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UltimateFireworksController>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
