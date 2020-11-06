using IPA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UltimateFireworks
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class UltimateFireworksController : MonoBehaviour, IInitializable
    {
        [Inject]
        FireworksController _fireworksController;

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            Plugin.Log?.Debug($"{name}: Awake()");
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
        }

        public void Initialize()
        {
            this._fireworksController.SetField("_spawnSize", this._fireworksController.GetField<Vector3, FireworksController>("_spawnSize") * 0.4f);
            this._fireworksController.SetField("_minSpawnInterval", 0.16f);
            this._fireworksController.SetField("_maxSpawnInterval", 0.2f);
            this._fireworksController.transform.position = new Vector3(this._fireworksController.transform.position.x, 0f, 0f);
#if DEBUG
            this._fireworksController.enabled = true;
            this.StartCoroutine(this._fireworksController.SpawningCoroutine());
#endif
        }
        #endregion
    }
}
