using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger.Console
{
    public class Console : MonoBehaviour
    {
        private struct LogEntry
        {
            public LogType Type;
            public string Message;
            public string StackTrace;
            public int DuplicatedCount;
        }

        [Flags]
        private enum LogTypeToShow
        {
            LOG = 0,
            WARNING = 1,
            ERROR = 2
        }

        private LogTypeToShow logTypesToShow = LogTypeToShow.LOG | LogTypeToShow.WARNING | LogTypeToShow.ERROR;

        private LogItem[] logItems;
        private List<LogEntry> logRegistry = new List<LogEntry>();
        private List<LogEntry> collapsedEntries = new List<LogEntry>();

        private bool isCollapsed;


        private void Awake()
        {
            Application.logMessageReceived += LogMessageReceived;
            logItems = GetComponentsInChildren<LogItem>(true);
        }

        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            RegisterEntry(condition, stackTrace, type);
            SetItemsContent();
        }

        private void RegisterEntry(string condition, string stackTrace, LogType type)
        {
            LogEntry entry = collapsedEntries.Find(x => (x.Message == condition && x.StackTrace == stackTrace && x.Type == type));

            //Not Found
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
        }

        private void SetItemsContent()
        {
            List<LogEntry> entryList = isCollapsed ? collapsedEntries : logRegistry;

            if (entryList.Count < logItems.Length)
            {
                LogEntry entry = entryList[entryList.Count - 1];
                LogItem item = logItems[entryList.Count];
                item.SetContent(entry.Type, entry.Message, entry.StackTrace, entry.DuplicatedCount);
                item.ShowItem();
            }
            else
            {
                int logItemIndex = logItems.Length;
                foreach (LogItem item in logItems)
                {
                    LogEntry entry = entryList[entryList.Count - logItemIndex--];
                    item.SetContent(entry.Type, entry.Message, entry.StackTrace, entry.DuplicatedCount);
                }
            }
        }

        public void Clear()
        {
            logRegistry.Clear();
            collapsedEntries.Clear();

            foreach(LogItem item in logItems)
            {
                item.HideItem();
            }
        }

        public void Collapse()
        {
            isCollapsed = !isCollapsed;
        }

        public void ShowLogs()
        {
            logTypesToShow = logTypesToShow.HasFlag(LogTypeToShow.LOG) 
                ? logTypesToShow &= ~LogTypeToShow.LOG 
                : logTypesToShow |= LogTypeToShow.LOG;
        }
    }
}

