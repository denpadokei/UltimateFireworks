using HarmonyLib;
using IPA.Utilities;
using System;
using System.Linq;
using UltimateFireworks.Configuration;
using UnityEngine;

namespace UltimateFireworks.HarmonyPatches
{
    [HarmonyPatch(typeof(FireworkItemController), nameof(FireworkItemController.InitializeParticleSystem))]
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

        internal static void Postfix(FireworkItemController __instance)
        {
            try {
                var sparkColor = randomSparkColorPicker.PickRandomObject();
                var lightColor = randomLightColorPicker.PickRandomObject();
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").bypassReverbZones = true;
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").minDistance = 200f;
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").volume = 1.0f;
                __instance.GetField<AudioSource, FireworkItemController>("_audioSource").reverbZoneMix = 1.1f;
                __instance.SetField("_lightsColor", lightColor);
                __instance.SetField("_randomizeColor", false);
                __instance.SetField("_randomizeSpeed", true);
                var particles = __instance.GetField<FireworkItemController.FireworkItemParticleSystem[], FireworkItemController>("_particleSystems");
                foreach (var particleSystem in particles) {
                    if (particleSystem._isSubemitter) {
                        continue;
                    }
                    var particle = particleSystem._particleSystem;
                    particle.transform.localScale = new Vector3(PluginConfig.Instance.Scale, PluginConfig.Instance.Scale, PluginConfig.Instance.Scale);

                    var limit = particle.limitVelocityOverLifetime;
                    limit.enabled = true;
                    limit.dampen = 0.05f;
                    limit.separateAxes = false;
                    limit.multiplyDragByParticleSize = false;
                    limit.multiplyDragByParticleVelocity = true;
                    limit.limitX = new ParticleSystem.MinMaxCurve(1f, 0f);
                    limit.limitY = new ParticleSystem.MinMaxCurve(1f, 0f);
                    limit.limitZ = new ParticleSystem.MinMaxCurve(1f, 0f);

                    var emmistion = particle.emission;
                    if (emmistion.enabled) {
                        var rateOverTime = emmistion.rateOverTime;
                        emmistion.rateOverTime = 3000f;
                    }
                    

                    var renderer = particle.GetComponent<ParticleSystemRenderer>();
                    if (PluginConfig.Instance.FireEnable) {
                        renderer.renderMode = ParticleSystemRenderMode.Billboard;
                    }
                    else {
                        renderer.renderMode = ParticleSystemRenderMode.None;
                    }
                    var trail = particle.trails;
                    if (PluginConfig.Instance.TraileEnable) {
                        trail.enabled = true;
                        trail.lifetime = new ParticleSystem.MinMaxCurve(0.1f, 2f);
                        renderer.trailMaterial = Default;
                    }
                    else {
                        trail.enabled = false;
                    }

                    var rotationOverLifetime = particle.rotationOverLifetime;
                    rotationOverLifetime.separateAxes = true;

                    var orbital = particle.velocityOverLifetime;
                    if (0 < PluginConfig.Instance.Radial) {
                        orbital.enabled = true;
                        orbital.space = ParticleSystemSimulationSpace.World;
                        orbital.orbitalY = 10f;
                        orbital.radial = PluginConfig.Instance.Radial;
                    }
                    else {
                        orbital.enabled = false;
                    }
                    

                    var main = particle.main;
                    var color = main.startColor;
                    color.color = sparkColor;
                    color.colorMax = sparkColor;
                    main.startColor = color;
                    main.gravityModifierMultiplier = PluginConfig.Instance.GravityModifierMultiplier;
                    main.simulationSpace = ParticleSystemSimulationSpace.Local;
                    main.scalingMode = ParticleSystemScalingMode.Local;
                    main.maxParticles = 850;
                    main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
                    var colision = particle.collision;
                    if (PluginConfig.Instance.Refrect) {
                        colision.enabled = true;
                        colision.type = ParticleSystemCollisionType.World;
                    }
                    else {
                        colision.enabled = false;
                    }
                    
                }
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
