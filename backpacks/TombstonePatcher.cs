//This file is courtesy of the creator of the original Valheim Inventory Slots mod :)
using backpacks;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimInventorySlots
{
    [HarmonyPatch(typeof(Player), "CreateTombStone")]
    public class TombstonePatcher
    {
        static void Prefix(Player __instance)
        {

            if (!Backpacks.GKInstalled)
            {
                Player that = __instance;
                GameObject additionalTombstone = UnityEngine.Object.Instantiate(that.m_tombstone, that.GetCenterPoint() + Vector3.up * 2.5f, that.transform.rotation);
                additionalTombstone.gameObject.transform.localScale -= new Vector3(.5f, .5f, .5f);
                Container graveContainer = additionalTombstone.GetComponent<Container>();
                that.UnequipAllItems();
                Func<ItemDrop.ItemData, bool> predicate = new Func<ItemDrop.ItemData, bool>(item => item.m_gridPos.y >= 4 && !item.m_shared.m_questItem && !item.m_equiped);
                foreach (var item in that.GetInventory().GetAllItems().Where(predicate).ToArray())
                {
                    if (item.m_gridPos.y >= 4)
                    {
                        graveContainer.GetInventory().AddItem(item);
                        that.GetInventory().RemoveItem(item);
                    }
                }
            }

        }
    }
}
