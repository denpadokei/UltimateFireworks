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
        public static float Scale { get; set; }
        public static bool FireEnable { get; set; }
        public static bool TraileEnable { get; set; }
        public static float Radial { get; set; }
        public static float GravityModifierMultiplier { get; set; }
        public static bool Refrect { get; set; }

        private static Material _default;

        private static Material Default => _default ?? (_default = Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(x => x.name == "FireworkExplosion"));

        internal static void Postfix(FireworkItemController __instance)
        {
            try {
                var h = s_colorBaseValue * s_colorIndex.Next(0, s_colorCount);
                var sparkColor = Color.HSVToRGB(h, 1, 1);
                h = s_colorBaseValue * s_colorIndex.Next(0, s_colorCount);
                var lightColor = Color.HSVToRGB(h, 1, 1);
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
                    particle.transform.localScale = new Vector3(Scale, Scale, Scale);

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
                    if (FireEnable) {
                        renderer.renderMode = ParticleSystemRenderMode.Billboard;
                    }
                    else {
                        renderer.renderMode = ParticleSystemRenderMode.None;
                    }
                    var trail = particle.trails;
                    if (TraileEnable) {
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
                    if (0 < Radial) {
                        orbital.enabled = true;
                        orbital.space = ParticleSystemSimulationSpace.World;
                        orbital.orbitalY = 10f;
                        orbital.radial = Radial;
                    }
                    else {
                        orbital.enabled = false;
                    }


                    var main = particle.main;
                    var color = main.startColor;
                    color.color = sparkColor;
                    color.colorMax = sparkColor;
                    main.startColor = color;
                    main.gravityModifierMultiplier = GravityModifierMultiplier;
                    main.simulationSpace = ParticleSystemSimulationSpace.Local;
                    main.scalingMode = ParticleSystemScalingMode.Local;
                    main.maxParticles = 850;
                    main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
                    var colision = particle.collision;
                    if (Refrect) {
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

        private static readonly System.Random s_colorIndex = new System.Random(Environment.TickCount);
        private static readonly Color s_baseColor = Color.red;
        private static readonly float s_colorBaseValue = 1f / s_colorCount;
        private const int s_colorCount = 180000;
    }
}
