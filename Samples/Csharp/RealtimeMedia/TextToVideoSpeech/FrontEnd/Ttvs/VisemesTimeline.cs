/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using System.Collections.Generic;

namespace FrontEnd.Ttvs
{
    /// <summary>
    /// A representation of a visemes timeline
    /// </summary>
    class VisemesTimeline
    {
        private int _referenceTimeStamp = 0;

        private readonly SortedDictionary<int, int> _visemesIds = new SortedDictionary<int, int>();

        public int Length => _referenceTimeStamp;

        /// <summary>
        /// Add a viseme id 
        /// </summary>
        /// <param name="visemeId">The id of the viseme</param>
        /// <param name="durationInMs">The duration in ms starting from the previous viseme</param>
        public void Add(int visemeId, int durationInMs)
        {
            _referenceTimeStamp += durationInMs;

            _visemesIds.Add(_referenceTimeStamp, visemeId);
        }

        /// <summary>
        /// Get the viseme id for the given timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public int Get(int timestamp)
        {
            var visemeId = 0;
            foreach (var key in _visemesIds.Keys)
            {
                if (timestamp < key)
                {
                    _visemesIds.TryGetValue(key, out visemeId);
                    break;
                }
            }
            return visemeId;
        }
    }
}
