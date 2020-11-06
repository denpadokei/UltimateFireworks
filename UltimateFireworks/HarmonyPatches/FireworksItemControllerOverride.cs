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
    [HarmonyPatch(typeof(MemoryPool<FireworkItemController>), nameof(MemoryPool<FireworkItemController>.Spawn))]
    public class FireworksItemControllerOverride
    {
        internal static void Postfix(ref FireworkItemController __result)
        {
            __result.GetField<AudioSource, FireworkItemController>("_audioSource").bypassReverbZones = true;
            __result.GetField<AudioSource, FireworkItemController>("_audioSource").minDistance = float.MaxValue;
            __result.GetField<AudioSource, FireworkItemController>("_audioSource").volume = 1.0f;
            __result.GetField<AudioSource, FireworkItemController>("_audioSource").reverbZoneMix = 1.1f;
            __result.SetField("_numberOfParticles", 20000);
        }
    }
}
