using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Debugger.Console
{
    public class LogItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logType;
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private TextMeshProUGUI logStackTraceText;
        [SerializeField] private TextMeshProUGUI duplicatedText;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowItem()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void HideItem()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void SetContent(LogType type, string logMessage, string stackTraceMessage, int duplicatedTimes)
        {
            switch (type)
            {
                case LogType.Log:
                    logType.text = "[LOG]";
                    break;

                case LogType.Warning:
                    logType.text = "[WARNING]";
                    break;

                case LogType.Error:
                    logType.text = "[ERROR]";
                    break;
            }

            logText.text = logMessage;
            //logStackTraceText.text = stackTraceMessage;
            duplicatedText.text = duplicatedTimes.ToString();
        }
    }
}
