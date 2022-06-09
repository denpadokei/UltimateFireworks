using HarmonyLib;

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
