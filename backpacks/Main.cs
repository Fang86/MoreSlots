using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ValheimInventorySlots;

namespace backpacks
{
    [BepInPlugin(MID, NAME, VERSION)]
    public class Backpacks : BaseUnityPlugin
    {
        private const string MID = "valheim.mod.moreslots";
        private const string NAME = "More Slots";
        private const string VERSION = "1.1.6";
        private const string author = "Fang86";

        public static bool GKInstalled;

        static Assembly assem = typeof(Backpacks).Assembly;
        public static string fpath = assem.Location;
        public static string path = fpath.Remove(fpath.Length - 13);

        private static ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "moreslots.cfg"), true);
        private static ConfigEntry<int> rows = configFile.Bind("General", "rows", 6, "Number of rows in the player's inventory grid");
        //private static ConfigEntry<int> columns = configFile.Bind("General", "columns", 8, "Number of columns in the player's inventory grid. It is recommended to leave this number at 8");




        void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

            FixGuiFrame.ExtraRows = Backpacks.rows.Value - 4;

            Logger.LogInfo($"More Slots mod - Created by {author}");

            //string fpath = Assembly.GetExecutingAssembly().Location;
            //Logger.LogInfo($"First, path is {fpath} with length of " + fpath.Length);
            //string path = fpath.Remove(fpath.Length - 13);
            //Logger.LogInfo($"Now, path is {path} with length of " + path.Length);

            if (File.Exists($@"{path}Gravekeeper.dll"))
            {
                Logger.LogInfo("Gravekeeper found");
                GKInstalled = true;
            } 
            else
            {
                Logger.LogInfo("Gravekeeper not found");
                GKInstalled = false;
            }
        }

        [HarmonyPatch(typeof(Player), "Awake")]
        public static class GridExtension
        {
            private static void Prefix(ref Player __instance)
            {
                int rows = Backpacks.rows.Value;
                int columns = 8; //Backpacks.columns.Value;

                Traverse.Create(__instance).Field("m_inventory").SetValue(new Inventory("Inventory", null, columns, rows));

                

            }
        }

        /*
        [HarmonyPatch(typeof(InventoryGrid), "UpdateGui")]

        public static class ModifyBackground
        {
            private static void Postfix(ref InventoryGrid __instance, Player player)
            {
                if (__instance.name == "PlayerGrid")
                {

                    float addedRows = Backpacks.rows.Value - 4;
                    float offset = -35 * addedRows;

                    RectTransform gridBkg = ModifyBackground.GetOrCreateBackground(__instance, "ExtInvGrid");
                    gridBkg.anchoredPosition = new Vector2(0f, offset);
                    gridBkg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 590f);
                    gridBkg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300f + 75 * addedRows);
                } 
                
                if (__instance.name == "ContainerGrid")
                {
                    var bkg = __instance.transform.parent.Find("Bkg").gameObject;
                    Transform bkgT = bkg.transform;
                    bkgT.position = new Vector3(33 + 295, 550 - 75 * (Backpacks.rows.Value - 4), 0);


                    //UnityEngine.Debug.Log("Attempted move");
                }
            }



            //Credit for the function below goes to Randy Knapps!
            private static RectTransform GetOrCreateBackground(InventoryGrid __instance, string name)
            {
                var existingBkg = __instance.transform.parent.Find(name);
                if (existingBkg == null)
                {
                    var bkg = __instance.transform.parent.Find("Bkg").gameObject;
                    var background = GameObject.Instantiate(bkg, bkg.transform.parent);
                    background.name = name;
                    background.transform.SetSiblingIndex(bkg.transform.GetSiblingIndex() + 1);
                    existingBkg = background.transform;
                }

                return existingBkg as RectTransform;
            }
        }*/

        /*[HarmonyPatch(typeof(Inventory), "MoveInventoryToGrave")]

        public static class GraveCreator
        {
            public static void Prefix(ref Inventory __instance)
            {
                UnityEngine.Debug.Log("MoveInventoryToGrave called");
                
                Inventory NewGrave = new Inventory("NewGrave", null, 8, 8);

                List<ItemDrop.ItemData> pinv = (List<ItemDrop.ItemData>)Traverse.Create(__instance).Field("m_inventory").GetValue();
                List<ItemDrop.ItemData> nginv = (List<ItemDrop.ItemData>)Traverse.Create(NewGrave).Field("m_inventory").GetValue();

                Traverse.Create(NewGrave).Field("m_width").SetValue( Traverse.Create(__instance).Field("m_width").GetValue() );
                Traverse.Create(NewGrave).Field("m_height").SetValue( Traverse.Create(__instance).Field("m_height").GetValue());
                foreach (ItemDrop.ItemData itemData in pinv)
                {
                    if (!itemData.m_shared.m_questItem && !itemData.m_equiped)
                    {
                        nginv.Add(itemData);
                    }
                }
                pinv.RemoveAll((ItemDrop.ItemData x) => !x.m_shared.m_questItem && !x.m_equiped);
                //__instance.Changed();
                //NewGrave.Changed();

                UnityEngine.Debug.Log("MoveInventoryToGrave ended");
            }
        }*/


    }
}
