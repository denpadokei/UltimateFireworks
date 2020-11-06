using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using HarmonyLib;
using System.Reflection;

namespace UltimateFireworks
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        internal static Harmony _harmony;
        internal const string HARMONY_ID = "com.github.denpadokei.UltimateFireworks";

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            Log.Info("UltimateFireworks initialized.");
            _harmony = new Harmony(HARMONY_ID);
            zenjector.OnMenu<Installer.Installer>();
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            ApplyHarmonyPatches();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }

        public static void ApplyHarmonyPatches()
        {
            try {
                _harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }
    }
}
