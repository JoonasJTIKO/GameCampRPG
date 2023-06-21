using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public interface IInteractable
    {
        bool InRange 
        { 
            get;
            set;
        }

        void Interact();
    }
}
