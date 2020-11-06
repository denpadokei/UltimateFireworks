using HarmonyLib;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UltimateFireworks.HarmonyPatches
{
    [HarmonyPatch(typeof(MemoryPool<object>), nameof(MemoryPool<object>.Spawn))]
    public class FireworksItemControllerOverride
    {
        internal static void Postfix(ref object __result)
        {
            if (__result is FireworkItemController controller) {
                controller.GetField<AudioSource, FireworkItemController>("_audioSource").bypassReverbZones = true;
                controller.GetField<AudioSource, FireworkItemController>("_audioSource").minDistance = float.MaxValue;
                controller.GetField<AudioSource, FireworkItemController>("_audioSource").volume = 1.0f;
                controller.GetField<AudioSource, FireworkItemController>("_audioSource").reverbZoneMix = 1.1f;
                controller.SetField("_numberOfParticles", 20000);
            }
        }
    }
}
