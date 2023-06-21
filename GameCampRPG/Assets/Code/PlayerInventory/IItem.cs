using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public interface IItem
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Amount { get; set; }
        int MaxAmount { get; set; }
        bool IsUnique { get; set; }
        Sprite Icon { get; set; }
    }
}
