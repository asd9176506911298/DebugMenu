using System;
using UnityEngine;
using BepInEx;
using BepInEx.Unity.Mono;
using BepInEx.Configuration;
using HarmonyLib;
using Mortal.Battle;
using System.Reflection;

namespace DebugMenu
{
    [BepInPlugin("DebugMenu", "活俠傳作弊測試選單", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        private ConfigEntry<KeyCode> MenuToggleKey;
        GUIStyle myStyle = new GUIStyle();
        

        private bool showMenu = false;
        private bool isspeed = false;
        private float speed = 1;
        private string speedInput = "1";
        private bool resource = false;
        private bool day = false;
        private bool testAnimation = false;
        private bool winLose = false;
        public bool dice = false;
        public int diceNumber = 50;
        private string diceInput = "50";

        private Rect windowRect;

        private void Awake()
        {
            Debug.Log("活俠傳作弊測試選單");
            Instance = this;

            MenuToggleKey = Config.Bind<KeyCode>("DebugMenu", "MenuToggleKey", KeyCode.F1, "Menu Toggle Key");

            Harmony.CreateAndPatchAll(typeof(RollDice));

            // Initialize window size based on screen dimensions
            float width = Screen.width * 0.5f;
            float height = Screen.height * 0.5f;
            windowRect = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
        }

        private void onDestroy()
        {
            Harmony.UnpatchAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(MenuToggleKey.Value))
            {
                showMenu = !showMenu;
            }

            if (resource)
            {
                SetActive("[UI]/TopPanel/StatusPanel/TestPanel", true);
                SetActive("[UI]/TopPanel/StatusPanel/TestPanel/Flags", true);
                SetActive("[UI]/TopPanel/StatusPanel/TestPanel/Flags (1)", true);
            }
            else
            {
                SetActive("[UI]/TopPanel/StatusPanel/TestPanel", false);
            }

            if (isspeed)
            {
                setTimeScale();
            }
            else
            {
                Time.timeScale = 1;
            }

            if (day)
            {
                SetActive("[UI]/MainUI/Layer_1/TestDayButton", true);
                SetActive("[UI]/MainUI/Layer_1/TestNightButton", true);
            }
            else
            {
                SetActive("[UI]/MainUI/Layer_1/TestDayButton", false);
                SetActive("[UI]/MainUI/Layer_1/TestNightButton", false);
            }

            if (testAnimation)
            {
                SetActive("[UI]/MainUI/Layer_5/TestAnimationPanel", true);
            }
            else
            {
                SetActive("[UI]/MainUI/Layer_5/TestAnimationPanel", false);
            }

            if (winLose)
            {
                SetActive("[UI]/TopPanel/MenuPanel/TestWin", true);
                SetActive("[UI]/TopPanel/MenuPanel/TestLose", true);
            }
            else
            {
                SetActive("[UI]/TopPanel/MenuPanel/TestWin", false);
                SetActive("[UI]/TopPanel/MenuPanel/TestLose", false);
            }
        }

        private void setTimeScale()
        {
            if (speed > 0.0f)
                Time.timeScale = speed;
        }

        private void SetActive(string path, bool active)
        {
            GameObject obj = GameObject.Find(path);
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }

        private void OnGUI()
        {
            if (showMenu)
            {
                windowRect = GUI.Window(123456, windowRect, DoMyWindow, "活俠傳作弊測試選單 Made by Yuki.kaco");
            }
        }

        public void DoMyWindow(int windowID)
        {

            myStyle.fontSize = 14;
            myStyle.normal.textColor = Color.white;
            GUILayout.BeginArea(new Rect(10, 20, windowRect.width - 20, windowRect.height - 30));
            {
                GUILayout.BeginVertical();
                {
                    isspeed = GUILayout.Toggle(isspeed, "開啟加速");
                    GUILayout.Label("   加速速度", myStyle);
                    speedInput = GUILayout.TextField(speedInput);
                    float.TryParse(speedInput, out speed);
                    dice = GUILayout.Toggle(dice, "控制骰子");
                    GUILayout.Label("    骰子數字", myStyle);
                    diceInput = GUILayout.TextField(diceInput);
                    int.TryParse(diceInput, out diceNumber);
                    resource = GUILayout.Toggle(resource, "修改資源");
                    day = GUILayout.Toggle(day, "測試白天晚上");
                    testAnimation = GUILayout.Toggle(testAnimation, "測試動畫");
                    winLose = GUILayout.Toggle(winLose, "直接勝利/失敗");
                    if (GUILayout.Button("團體戰玩家死亡"))
                        Traverse.Create(GameLevelManager.Instance).Method("ShowGameOver", GameOverType.PlayerDie, true).GetValue();
                    if (GUILayout.Button("團體戰友方獲勝"))
                        Traverse.Create(GameLevelManager.Instance).Method("ShowGameOver", GameOverType.FriendWin, true).GetValue();
                    if (GUILayout.Button("團體戰敵方獲勝"))
                        Traverse.Create(GameLevelManager.Instance).Method("ShowGameOver", GameOverType.EnemyWin, true).GetValue();
                    if (GUILayout.Button("團體戰時間到"))
                        Traverse.Create(GameLevelManager.Instance).Method("ShowGameOver", GameOverType.Timeout, true).GetValue();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            GUI.DragWindow();
        }
    }
}
