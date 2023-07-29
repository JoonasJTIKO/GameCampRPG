using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public interface IInteractable
    {
        void Interact();

        void PlayerEnterRange(bool entered);
    }
}
