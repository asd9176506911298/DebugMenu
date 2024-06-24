using HarmonyLib;

namespace DebugMenu
{
    public class RollDice
    {
        [HarmonyPrefix, HarmonyPatch(typeof(Mortal.Story.CheckPointManager), "Dice")]
        public static bool modifyDiceNumnber(ref int random)
        {
            if (Plugin.Instance.dice)
                if (Plugin.Instance.diceNumber > 0 && Plugin.Instance.diceNumber < 100)
                    random = Plugin.Instance.diceNumber;

            return true;
        }
    }
}
