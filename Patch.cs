using HarmonyLib;
using Mortal.Core;
using UnityEngine;
using Unity.Rendering;
using UnityEngine.Rendering;
using System;
using BepInEx;
using System.Security.AccessControl;

namespace DebugMenu
{
    public class Patch
    {
        [HarmonyPrefix, HarmonyPatch(typeof(Mortal.Story.CheckPointManager), "Dice")]
        public static bool modifyDiceNumnber(ref int random)
        {
            if (Plugin.Instance.dice)
                if (Plugin.Instance.diceNumber > 0 && Plugin.Instance.diceNumber < 100)
                    random = Plugin.Instance.diceNumber;

            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Mortal.Core.DevelopmentOnly), "Start")]
        public static bool DevelopmentOnly(ref Mortal.Core.DevelopmentOnly __instance)
        {
            bool isActive = Traverse.Create(__instance).Field("_active").GetValue<bool>();
            if (!Debug.isDebugBuild && isActive)
                __instance.gameObject.SetActive(false);
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Mortal.Combat.CombatResultTestButton), "Start")]
        public static bool TestButton(ref Mortal.Combat.CombatResultTestButton __instance)
        {
            if (!Debug.isDebugBuild)
                __instance.gameObject.SetActive(false);
            return false;
        }

        //改為測試
        [HarmonyPostfix, HarmonyPatch(typeof(Debug), "isDebugBuild", MethodType.Getter)]
        public static void isDebugBuild(ref bool __result)
        {
            __result = true;
        }

        //左下角FPS
        [HarmonyPrefix, HarmonyPatch(typeof(FpsDisplayController), "Update")]
        public static bool FpsDisplayController()
        {
            return false;
        }

        //左上角資源監視器Profiler
        [HarmonyPrefix, HarmonyPatch(typeof(ProfilerRecorderController), "OnGUI")]
        public static bool ProfilerRecorderController()
        {
            return false;
        }
    }
}
