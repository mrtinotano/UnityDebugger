using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Debugger.Console
{
    public class LogItem : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image logType;
        //[SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private Text logText;
        [SerializeField] private TextMeshProUGUI duplicatedText;
        public string StackTraceText { get; private set; }
        public int LogEntryIndex { get; private set; } = -1;
        public bool IsVisible => canvasGroup.alpha == 1f;

        private CanvasGroup canvasGroup;
        private Console console;
        private Color backgroundDefaultColor;

        private bool isSelected;

        private readonly Color selectedColor = new Color(0.17f, 0.36f, 0.52f);

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            console = GetComponentInParent<Console>();
            GetComponent<Button>().onClick.AddListener(()=>
            {
                SetSelected(!isSelected);
                console.SetSelectedItemState(this);
            });

            backgroundDefaultColor = background.color;
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

            if(isSelected)
                SetSelected(false);
        }

        public void SetContent(LogType type, string logMessage, string stackTraceMessage, int duplicatedTimes, int entryIndex)
        {
            switch (type)
            {
                case LogType.Log:
                    logType.color = Color.white;
                    break;

                case LogType.Warning:
                    logType.color = Color.yellow;
                    break;

                case LogType.Error:
                    logType.color = Color.red;
                    break;
            }

            logText.text = logMessage;
            StackTraceText = stackTraceMessage;
            duplicatedText.text = duplicatedTimes.ToString();
            LogEntryIndex = entryIndex;

            if (canvasGroup.alpha == 0)
                ShowItem();
        }

        public void SetSelected(bool selected)
        {
            if (isSelected == selected)
                return;

            background.color = selected ? selectedColor : backgroundDefaultColor;
            isSelected = selected;
        }
    }
}
