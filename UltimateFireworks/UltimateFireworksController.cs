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
        private FireworksController _fireworksController;
        private ISoundLoader _soundLoader;
        public string SoundSet { get; set; }
        internal PluginConfig.FireWorksMode _mode;
        internal PluginConfig.FireWorksMode Mode
        {
            get => this._mode;
            set
            {
                this._mode = value;
                this.SetMode(value);
            }
        }
        [Inject]
        internal void Constract(FireworksController fireworksController, ISoundLoader soundLoader)
        {
            this._fireworksController = fireworksController;
            this._soundLoader = soundLoader;
        }

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Slash)) {
                SetEnableFireWorksController();
            }
        }
        public void Initialize()
        {
            this.SoundSet = PluginConfig.Instance.SoundSet;
            this.Mode = PluginConfig.Instance.Mode;
            var collider = Resources.FindObjectsOfTypeAll<Collider>().FirstOrDefault(x => x.name == "GroundCollider");

            if (collider != null) {
                if (collider is BoxCollider box) {
                    box.size = new Vector3(720f, box.size.y, 800f);
                }
            }
            this.SetMode(this.Mode);
        }
        #endregion

        private void SetMode(PluginConfig.FireWorksMode fireWorksMode)
        {
            var spawnSize = this._fireworksController.GetField<Vector3, FireworksController>("_spawnSize") * 0.4f;
            if (fireWorksMode == PluginConfig.FireWorksMode.InSide) {
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

        [AffinityPatch(typeof(FireworkItemController), nameof(FireworkItemController.InitializeParticleSystem))]
        [AffinityPrefix]
        private void InitializeParticleSystem(AudioClip[] ____explosionClips, ref RandomObjectPicker<AudioClip> ____randomAudioPicker)
        {
            if (this.SoundSet == "Default") {
                if (____randomAudioPicker._objects.SequenceEqual(____explosionClips)) {
                    return;
                }
                ____randomAudioPicker = new RandomObjectPicker<AudioClip>(____explosionClips, 0.2f);
            }
            else {
                if (!this._soundLoader.Sounds.TryGetValue(Path.Combine(this._soundLoader.DataPath, this.SoundSet), out var sounds)) {
                    return;
                }
                if (____randomAudioPicker._objects.SequenceEqual(sounds)) {
                    return;
                }
                ____randomAudioPicker = new RandomObjectPicker<AudioClip>(sounds, 0.2f);
            }
        }

        private void SetEnableFireWorksController()
        {
            if (this._fireworksController) {
                this._fireworksController.enabled = this._fireworksController.enabled;
            }
        }
    }
}
