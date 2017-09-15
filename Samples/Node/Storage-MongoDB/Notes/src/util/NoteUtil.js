var moment = require('moment');


function NoteUtil() {}

/**
  Convert the timestamp Date object to a human readable format
**/
NoteUtil.formatDate = function(date, localTimestamp) {
  let dateWrapper = moment(date);
  let localZoneWrapper = moment.parseZone(localTimestamp);
  dateWrapper.utcOffset(localZoneWrapper.utcOffset());
  return dateWrapper.format("MMM D, YYYY HH:mm");
}

/**
 * Given number of milliseconds elapsed since Jan 1st 1970 00:00:00 UTC,
 * convert it into universal ticks.
 */
NoteUtil.dateToTicks = function (date) {
  // epochTicks is the value of ticks from Jan 1st 1900 to Jan 1st 1970.
  const epochTicks = 621355968000000000;
  // There are 10,000 ticks per millisecond.
  const ticksPerMs = 10000;
  
  return ((date * ticksPerMs) + epochTicks);
}

/**
 * Given universal ticks,
 * convert it to number of milliseconds elapsed since Jan 1 1970 00:00:00 UTC.
 */
NoteUtil.ticksToDate = function (ticks) {
  // epochTicks is the value of ticks from Jan 1st 1900 to Jan 1st 1970.
  const epochTicks = 621355968000000000;
  // There are 10,000 ticks per millisecond.
  const ticksPerMs = 10000;

  return Math.floor((ticks - epochTicks) / ticksPerMs);
}

module.exports = NoteUtil;