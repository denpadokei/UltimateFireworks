using HarmonyLib;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using static UnityEngine.ParticleSystem;

namespace UltimateFireworks.HarmonyPatches
{
    [HarmonyPatch(typeof(MemoryPool<object>), nameof(MemoryPool<object>.Spawn))]
    public class FireworksItemControllerOverride
    {
        private static Material _default;

        private static Material Default
        {
            get
            {
                return _default ?? (_default = Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(x => x.name == "FireworkExplosion"));
            }
        }

        internal static void Postfix(ref object __result)
        {
            if (__result is FireworkItemController controller) {
                try {
                    var sparkColor = randomSparkColorPicker.PickRandomObject();
                    var lightColor = randomLightColorPicker.PickRandomObject();
                    controller.GetField<AudioSource, FireworkItemController>("_audioSource").bypassReverbZones = true;
                    controller.GetField<AudioSource, FireworkItemController>("_audioSource").minDistance = float.MaxValue;
                    controller.GetField<AudioSource, FireworkItemController>("_audioSource").volume = 1.0f;
                    controller.GetField<AudioSource, FireworkItemController>("_audioSource").reverbZoneMix = 1.1f;
                    controller.SetField("_numberOfParticles", 20000);
                    controller.SetField("_lightsColor", lightColor);
                    var particle = controller.GetField<ParticleSystem, FireworkItemController>("_particleSystem");
                    var trail = particle.trails;
                    trail.enabled = true;
                    trail.lifetime = 2.0f;
                    var renderer = particle.GetComponent<ParticleSystemRenderer>();
                    renderer.renderMode = ParticleSystemRenderMode.None;
                    renderer.trailMaterial = Default;
                    var main = particle.main;
                    var color = main.startColor;
                    color.color = sparkColor;
                    color.colorMax = sparkColor;
                    main.startColor = color;
                    main.gravityModifierMultiplier = 2f;
                    renderer.sortingOrder = 3;
                }
                catch (Exception e) {
                    Plugin.Log.Error(e);
                }
            }
        }

        static readonly Color[] lightColors = new Color[4]
        {
            Color.white,
            Color.red,
            Color.green,
            Color.blue
        };

        static readonly Color[] sparkColors = new Color[7]
        {
            new Color(0f, 0f, 1f, 1f),
            new Color(0f, 1f, 0f, 1f),
            new Color(1f, 0f, 0f, 1f),
            new Color(1f, 1f, 1f, 1f),
            new Color(0f, 0.753f, 1f, 1f),
            new Color(0.753f, 1f, 0f, 1f),
            new Color(1f, 0f, 753f, 1f)
        };

        static RandomObjectPicker<Color> randomSparkColorPicker;
        static RandomObjectPicker<Color> randomLightColorPicker;

        static FireworksItemControllerOverride()
        {
            randomSparkColorPicker = new RandomObjectPicker<Color>(sparkColors, 0.1f);
            randomLightColorPicker = new RandomObjectPicker<Color>(lightColors, 0.1f);
        }
    }
}
