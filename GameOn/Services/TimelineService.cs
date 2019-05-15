using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOn.Web.Services
{
    /// <summary>
    /// A Timeline service for saving and retrieving messages in a timeline
    /// </summary>
    public class TimelineService : ITimelineService
    {
        private readonly TimelineMessages _timelineMessages = new TimelineMessages();

        /// <summary>
        /// Add a message as string. Supports a format string and args
        /// </summary>
        /// <param name="format">The message, or a format string for the message</param>
        /// <param name="args">An optional param array of args for the format string</param>
        public void AddMessage(string format, params object[] args)
        {
            _timelineMessages.Add(format, args);
        }

        /// <summary>
        /// Gets all messages in the timeline ordered by Date and Time
        /// </summary>
        public IList<Tuple<DateTime, string>> Get()
        {
            return _timelineMessages.Get().OrderByDescending(m=>m.Item1).ToList();
        }
    }
}