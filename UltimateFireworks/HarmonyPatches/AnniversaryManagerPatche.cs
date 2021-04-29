using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateFireworks.HarmonyPatches
{
    [HarmonyPatch(typeof(AnniversaryManager), nameof(AnniversaryManager.StartFireworks))]
    public class AnniversaryManagerStartFireworksPatche
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(AnniversaryManager), nameof(AnniversaryManager.StopFireworks))]
    public class AnniversaryManagerStopFireworksPatche
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
