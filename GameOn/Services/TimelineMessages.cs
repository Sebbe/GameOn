using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameOn.Web.Services
{
    /// <summary>
    /// A collection of timestamped message strings
    /// </summary>
    public class TimelineMessages
    {
        /// <summary>
        /// The maximum number of messages to store in the timeline. Once this limit is reached, the oldest messages are evicted
        /// </summary>
        private const int TimelineMessageCapacity = 20;

        /// <summary>
        /// Threadsafe stack is used to hold the messages
        /// </summary>
        private static readonly ConcurrentQueue<Tuple<DateTime, string>> Messages = new ConcurrentQueue<Tuple<DateTime, string>>();

        /// <summary>
        /// Adds a message to the timeline. Supports an optional format string and param array of args
        /// </summary>
        public void Add(string format, params object[] args)
        {
            Messages.Enqueue(new Tuple<DateTime, string>(DateTime.Now, string.Format(format, args)));

            // evict oldest message/s beyond the capacity limit
            while (Messages.Count > TimelineMessageCapacity)
            {
                Tuple<DateTime, string> result;
                if (!Messages.TryDequeue(out result)) break;
            }
        }

        /// <summary>
        /// Gets all messages in the timeline as a List
        /// </summary>
        public List<Tuple<DateTime, string>> Get()
        {
            return Messages.ToList();
        }
    }
}

