using HarmonyLib;
using IPA.Utilities;
using System;
using System.Linq;
using UnityEngine;

namespace UltimateFireworks.HarmonyPatches
{
    [HarmonyPatch(typeof(FireworkItemController), nameof(FireworkItemController.Fire))]
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
        internal static void Prefix(FireworkItemController __instance)
        {
            try {
                var sparkColor = randomSparkColorPicker.PickRandomObject();
                var lightColor = randomLightColorPicker.PickRandomObject();
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").bypassReverbZones = true;
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").minDistance = float.MaxValue;
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").volume = 1.0f;
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").reverbZoneMix = 1.1f;
                __instance.SetField("_numberOfParticles", 20000);
                __instance.SetField("_lightsColor", lightColor);
                __instance.SetField("_lightFlashDuration", 8f);
                var particle = __instance.GetField<ParticleSystem, FireworkItemController>("_particleSystem");
                particle.transform.localScale = new Vector3(10f, 10f, 10f);
                var trail = particle.trails;
                trail.enabled = true;
                trail.lifetime = new ParticleSystem.MinMaxCurve(3f, 15f);
                var limit = particle.limitVelocityOverLifetime;
                limit.enabled = true;
                limit.dampen = 0.12f;
                limit.separateAxes = false;
                limit.multiplyDragByParticleSize = true;
                limit.multiplyDragByParticleVelocity = true;
                limit.limitX = new ParticleSystem.MinMaxCurve(1f, 0f);
                limit.limitY = new ParticleSystem.MinMaxCurve(1f, 0f);
                limit.limitZ = new ParticleSystem.MinMaxCurve(1f, 0f);
                var renderer = particle.GetComponent<ParticleSystemRenderer>();
                renderer.renderMode = ParticleSystemRenderMode.None;
                renderer.trailMaterial = Default;
                var main = particle.main;
                var color = main.startColor;
                color.color = sparkColor;
                color.colorMax = sparkColor;
                main.startColor = color;
                main.gravityModifierMultiplier = 2f;
                var light = particle.lights;
                light.enabled = true;
                light.ratio = 1f;
                light.useParticleColor = true;
                if (light.light == null) {
                    light.light = particle.gameObject.AddComponent<Light>();
                    light.light.type = LightType.Point;
                }
                light.light.intensity = 5f;
                light.light.range = 3f;
                light.maxLights = 0;
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
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
