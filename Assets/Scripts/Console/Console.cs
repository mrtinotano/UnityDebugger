using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger.Console
{
    public class Console : MonoBehaviour
    {
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private Text stackTrace;
        [SerializeField] private Button buttonToBottom;

        private struct LogEntry
        {
            public LogType Type;
            public string Message;
            public string StackTrace;
            public int DuplicatedCount;
            public int ListIndex;
        }

        private LogItem[] logItems;
        private List<LogEntry> logRegistry = new List<LogEntry>();
        private List<LogEntry> collapsedEntries = new List<LogEntry>();

        private List<LogEntry> currentEntryList => isCollapsed ? collapsedEntries : logRegistry;

        private bool isCollapsed;
        private bool isUsingScroll;
        private int scrollingStartIndex;


        private bool showLogs = true;
        private bool showWarnings = true;
        private bool showErrors = true;

        private LogItem selectedItem;
        private int selectedEntryIndex = -1;

        private const int maxLogsToShow = 999;
        private const float minScrollbarSize = 0.05f;

        

        private void Awake()
        {
            Application.logMessageReceived += LogMessageReceived;
            logItems = GetComponentsInChildren<LogItem>(true);
            scrollbar.onValueChanged.AddListener((value)=> OnScrollActivate());
            buttonToBottom.onClick.AddListener(()=> MoveToBottom());
        }

        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            RegisterEntry(condition, stackTrace, type);
            UpdateLogItems();
        }

        private void RegisterEntry(string condition, string stackTrace, LogType type)
        {
            UnityEngine.Profiling.Profiler.BeginSample("Test1");
            LogEntry entry = collapsedEntries.Find(x => (x.Message == condition && x.StackTrace == stackTrace && x.Type == type));

            //Entry exist
            if (!entry.Equals(default(LogEntry)))
            {
                entry.DuplicatedCount++;
            }
            else
            {
                entry = new LogEntry
                {
                    Type = type,
                    Message = condition,
                    StackTrace = stackTrace,
                    DuplicatedCount = 0
                };
                collapsedEntries.Add(entry);
            }

            logRegistry.Add(entry);
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void UpdateLogItems()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Test2");
            if (!showLogs && !showWarnings && !showErrors)
            {
                HideItems();
                return;
            }

            List<LogEntry> entryList = currentEntryList;
            
            if (selectedItem == null || isUsingScroll)
            {
                //Filter logs
                List<LogEntry> visibleLogs = new List<LogEntry>();

                int logEntryStartIndex = isUsingScroll ? scrollingStartIndex : entryList.Count - 1;

                for (int i = logEntryStartIndex; i >= 0; i--)
                {
                    LogEntry entry = entryList[i];

                    switch (entry.Type)
                    {
                        case LogType.Log:
                            if (!showLogs) continue;
                            break;
                        case LogType.Warning:
                            if (!showWarnings) continue;
                            break;
                        case LogType.Error:
                            if (!showErrors) continue;
                            break;
                    }

                    entry.ListIndex = i;
                    visibleLogs.Add(entry);

                    if (visibleLogs.Count == logItems.Length)
                        break;
                }

                bool updateSelectedItem = selectedItem != null &&
                                            isUsingScroll &&
                                            (selectedEntryIndex <= logEntryStartIndex && selectedEntryIndex > logEntryStartIndex - visibleLogs.Count);

                for (int i = visibleLogs.Count - 1, j = 0; i >= 0; i--, j++)
                {
                    LogEntry entry = visibleLogs[j];
                    LogItem item = logItems[i];
                    item.SetContent(entry.Type, entry.Message, entry.StackTrace, entry.DuplicatedCount, entry.ListIndex);

                    if (!updateSelectedItem)
                        continue;

                    if (entry.ListIndex == selectedEntryIndex)
                        item.SetSelected(true);
                }

            }

            //Update scrollbar size based on entries number
            if (logItems.Last().IsVisible)
            {
                if (scrollbar.size > minScrollbarSize)
                    scrollbar.size = (entryList.Count >= maxLogsToShow) ? minScrollbarSize : 1.0f - (float)entryList.Count / maxLogsToShow;

                if (isUsingScroll || selectedItem != null)
                    scrollbar.SetValueWithoutNotify(1.0f - ((float)scrollingStartIndex / entryList.Count));
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void HideItems()
        {
            foreach (LogItem item in logItems)
            {
                if (item.IsVisible)
                    item.HideItem();
            }

            ResetScrollbar();
        }

        private void ResetScrollbar()
        {
            scrollbar.size = 1.0f;
            scrollbar.value = 0.0f;
            isUsingScroll = false;
            buttonToBottom.gameObject.SetActive(false);
        }

        public void Clear()
        {
            logRegistry.Clear();
            collapsedEntries.Clear();

            HideItems();
        }

        public void Collapse()
        {
            isCollapsed = !isCollapsed;
        }

        public void ShowLogs()
        {
            showLogs = !showLogs;
            RefreshConsole();
        }

        public void ShowWarnings()
        {
            showWarnings = !showWarnings;
            RefreshConsole();
        }

        public void ShowErrors()
        {
            showErrors = !showErrors;
            RefreshConsole();
        }

        private void RefreshConsole()
        {
            HideItems();
            UpdateLogItems();
        }

        public void SetSelectedItemState(LogItem item)
        {
            UnityEngine.Profiling.Profiler.BeginSample("Test3");
            if (selectedItem == item)
            {
                selectedItem = null;
                stackTrace.text = string.Empty;
                selectedEntryIndex = -1;
                MoveToBottom();
                return;
            }

            selectedItem = item;
            stackTrace.text = item.StackTraceText;
            selectedEntryIndex = item.LogEntryIndex;

            SetScrollStartIndex();
            ShowBottomButton();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void OnScrollActivate()
        {
            SetScrollStartIndex();
            isUsingScroll = true;
            ShowBottomButton();
        }

        private void SetScrollStartIndex()
        {
            scrollingStartIndex = (int)((currentEntryList.Count - 1.0f) * (1.0f - scrollbar.value));

            if (scrollingStartIndex < logItems.Length - 1 && currentEntryList.Count >= logItems.Length)
                scrollingStartIndex = logItems.Length - 1;

            if (selectedItem != null)
                if (selectedItem.LogEntryIndex > scrollingStartIndex || selectedItem.LogEntryIndex < scrollingStartIndex - logItems.Length)
                    selectedItem.SetSelected(false);
        }

        private void ShowBottomButton()
        {
            if (!buttonToBottom.gameObject.activeInHierarchy)
                buttonToBottom.gameObject.SetActive(true);
        }

        private void MoveToBottom()
        {
            scrollbar.value = 0f;
            isUsingScroll = false;
            buttonToBottom.gameObject.SetActive(false);
        }

        int i = 0;
        private void Update()
        {
            Debug.Log("wefew" + i++);   
            Debug.LogWarning("wefew" + i++);   
            Debug.LogError("wefew" + i++);   
        }
    }
}

