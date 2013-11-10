"use strict";

define([], function () {

    function createTimeString(secondsAsString, includeHours, includeLeadingZeros) {
        includeLeadingZeros = (includeLeadingZeros !== false);
        var secNum = parseInt(secondsAsString, 10);
        var hours = Math.floor(secNum / 3600);
        var minutes = Math.floor((secNum - (hours * 3600)) / 60);
        var seconds = secNum - (hours * 3600) - (minutes * 60);

        if (!includeHours) {
            minutes += (hours * 60);
        }
        else if (includeLeadingZeros && hours < 10) {
            hours = "0" + hours;
        }

        if (includeLeadingZeros && minutes < 10) {
            minutes = "0" + minutes;
        }
        if (seconds < 10) {
            seconds = "0" + seconds;
        }

        var time = minutes + ':' + seconds;
        if (includeHours) {
            time = hours + ':' + time;
        }

        return time;
    }
    
    if (!String.prototype.format) {
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/\{(\d+)\}/g, function (match, number) {
                return (typeof args[number] !== "undefined")
                  ? args[number]
                  : match
                ;
            });
        };
    }

    if (!String.prototype.toHHMMSS) {
        String.prototype.toHHMMSS = function (includeLeadingZeros) {
            return createTimeString(this, true, includeLeadingZeros);
        };
    }
    if (!String.prototype.toMMSS) {
        String.prototype.toMMSS = function (includeLeadingZeros) {
            return createTimeString(this, false, includeLeadingZeros);
        };
    }
});