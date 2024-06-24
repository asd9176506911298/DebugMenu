using System;
using UnityEngine;
using BepInEx;
using BepInEx.Unity.Mono;
using BepInEx.Configuration;

namespace DebugMenu
{
    [BepInPlugin("DebugMenu", "活俠傳作弊測試選單", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<KeyCode> MenuToggleKey;

        GUIStyle myStyle;

        private bool showMenu = false;
        string numberInput = "1";
        private bool isspeed = false;
        private float speed = 1;
        private bool resource = false;
        private bool day = false;
        private bool testAnimation = false;
        private bool winLose = false;
        private bool dice = false;

        private Rect windowRect;

        private void Awake()
        {
            Debug.Log("活俠傳作弊測試選單");
            MenuToggleKey = Config.Bind<KeyCode>("DebugMenu", "MenuToggleKey", KeyCode.F1, "Menu Toggle Key");

            // Initialize window size based on screen dimensions
            float width = Screen.width * 0.5f;
            float height = Screen.height * 0.5f;
            windowRect = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
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
            myStyle = new GUIStyle();
            myStyle.fontSize = 18;
            myStyle.normal.textColor = Color.white;
            GUILayout.BeginArea(new Rect(10, 20, windowRect.width - 20, windowRect.height - 30));
            {
                GUILayout.BeginVertical();
                {
                    isspeed = GUILayout.Toggle(isspeed, "開啟加速");

                    GUILayout.Label("加速速度", myStyle);
                    numberInput = GUILayout.TextField(numberInput);
                    float.TryParse(numberInput, out speed);
                    resource = GUILayout.Toggle(resource, "修改資源");
                    day = GUILayout.Toggle(day, "測試白天晚上");
                    testAnimation = GUILayout.Toggle(testAnimation, "測試動畫");
                    winLose = GUILayout.Toggle(winLose, "直接勝利/失敗");
                    dice = GUILayout.Toggle(dice, "控制骰子");
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            GUI.DragWindow();
        }
    }
}
