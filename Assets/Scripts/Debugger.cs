using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

namespace Debugger
{
    public class Debugger : MonoBehaviour
    {
        private CanvasGroup currentGroup;

        public void ShowCanvasGroup(CanvasGroup canvasGroup)
        {
            HideCurrentGroup();

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            currentGroup = canvasGroup;
        }

        private void HideCurrentGroup()
        {
            if (currentGroup == null)
                return;

            currentGroup.alpha = 0f;
            currentGroup.interactable = false;
            currentGroup.blocksRaycasts = false;
        }
    }
}
