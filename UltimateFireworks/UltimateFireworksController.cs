using IPA.Utilities;
using SiraUtil.Affinity;
using System.IO;
using System.Linq;
using UltimateFireworks.Configuration;
using UltimateFireworks.Interfaces;
using UnityEngine;
using Zenject;

namespace UltimateFireworks
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class UltimateFireworksController : MonoBehaviour, IInitializable, IAffinity
    {
        [Inject]
        private readonly FireworksController _fireworksController;
        [Inject]
        private readonly ISoundLoader _soundLoader;

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Slash)) {
                this._fireworksController.enabled = !this._fireworksController.enabled;
            }
        }
        public void Initialize()
        {
            var spawnSize = this._fireworksController.GetField<Vector3, FireworksController>("_spawnSize") * 0.4f;
            var collider = Resources.FindObjectsOfTypeAll<Collider>().FirstOrDefault(x => x.name == "GroundCollider");

            if (collider != null) {
                if (collider is BoxCollider box) {
                    box.size = new Vector3(720f, box.size.y, 800f);
                }
            }
            if (PluginConfig.Instance.Mode == PluginConfig.FireWorksMode.InSide) {
                this._fireworksController.SetField("_spawnSize", new Vector3(spawnSize.x * 3f, spawnSize.y * 1.5f, spawnSize.x / 1.5f));
                this._fireworksController.SetField("_minSpawnInterval", 0.16f);
                this._fireworksController.SetField("_maxSpawnInterval", 0.5f);
                this._fireworksController.transform.position = new Vector3(this._fireworksController.transform.position.x, 20f, 30f);
            }
            else {
                this._fireworksController.SetField("_spawnSize", new Vector3(500f, 30f, 1f));
                this._fireworksController.SetField("_minSpawnInterval", 0.16f);
                this._fireworksController.SetField("_maxSpawnInterval", 0.5f);
                this._fireworksController.transform.position = new Vector3(0f, 120f, 100f);
            }
        }
        #endregion

        [AffinityPatch(typeof(FireworkItemController), nameof(FireworkItemController.InitializeParticleSystem))]
        [AffinityPrefix]
        private void InitializeParticleSystem(FireworkItemController __instance, AudioClip[] ____explosionClips)
        {
            if (PluginConfig.Instance.SoundSet == "Default" || !this._soundLoader.Sounds.TryGetValue(Path.Combine(this._soundLoader.DataPath, PluginConfig.Instance.SoundSet), out var sounds)) {
                return;
            }
            if (____explosionClips.SequenceEqual(sounds)) {
                return;
            }
            __instance.SetField("_explosionClips", sounds);
            __instance.SetField("_randomAudioPicker", new RandomObjectPicker<AudioClip>(sounds, 0.2f));
        }
    }
}
