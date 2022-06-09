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
