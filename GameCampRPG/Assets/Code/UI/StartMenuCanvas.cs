using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG.UI
{
    public class StartMenuCanvas : MenuCanvas
    {
        public void StartGame()
        {
            GameInstance.Instance.GetGameStateManager().Go(StateType.Village);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(initialSelectedObject);
            }
        }
    }
}
