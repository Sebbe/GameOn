using System;
using System.Collections.Generic;

namespace GameOn.Web.Services
{
    /// <summary>
    /// Defines a Timeline service for saving and retrieving messages in a timeline
    /// </summary>
    public interface ITimelineService
    {
        /// <summary>
        /// Add a message as string. Supports a format string and args
        /// </summary>
        /// <param name="format">The message, or a format string for the message</param>
        /// <param name="args">An optional param array of args for the format string</param>
        void AddMessage(string format, params object[] args);

        /// <summary>
        /// Gets all messages in the timeline ordered by Date and Time
        /// </summary>
        IList<Tuple<DateTime, string>> Get();
    }
}