using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//parent class 
public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    //subclasses must have these
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;
    public int stackSize = 99;

    public abstract ItemClass GetItem();
    public abstract ToolClass GetTool();
    public abstract MiscClass GetMisc();
    public abstract ConsumableClass GetConsumable();

}
