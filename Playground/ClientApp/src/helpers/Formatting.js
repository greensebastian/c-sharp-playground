import React from 'react';
import { FaSquare } from "react-icons/fa";
import { COLORS } from '../resources/Colors';

export class Formatting {
  static formattedTime(date) {
    let dateObj = new Date(date);
    let hours = dateObj.getHours();
    hours = this.withLeadingZero(hours);
    let minutes = dateObj.getMinutes();
    minutes = this.withLeadingZero(minutes);
    return hours + ":" + minutes;
  }

  static millisecondsToMinutes(ms) {
    return this.roundToDecimals(ms / 1000 / 60, 0) + " minutes";
  }

  static metersToKilometers(m) {
    return this.roundToDecimals(m / 1000, 2) + " km";
  }

  static flat(unit) {
    return unit.toString();
  }

  // Kinda ugly but useful and simple
  static roundToDecimals(value, decimals) {
    let multiplier = Math.pow(10, decimals);
    return Math.round(multiplier * value) / multiplier;
  }

  static captionSpan(color, title, index) {
    if (!index) index = 0;
    let style = {
      color: COLORS.GRAYSCALE.DARK,
      fontSize: "1rem"
    }
    return (
      <span key={index} className="m-0 mx-1 p-0 d-flex flex-row flex-nowrap align-items-center" style={style}><FaSquare color={color} className="mr-1" /> {title}</span>
    );
  }

  // Formats to local date string followed by "HH:MM"
  static formatDate(rawDate) {
    let date = new Date(rawDate);
    return date.toLocaleDateString() + " " + this.withLeadingZero(date.getHours()) + ":" + this.withLeadingZero(date.getMinutes());
  }

  // Formats a duration in milliseconds to days/hours/minutes, (10 mil) => "2 hours, 46 minutes"
  static formatDuration(rawDuration) {
    let minutes = Math.floor(rawDuration / 1000 / 60) % 60;
    let hours = Math.floor(rawDuration / 1000 / 60 / 60) % 24;
    let days = Math.floor(rawDuration / 1000 / 60 / 60 / 24);
    minutes = this.inPlural(minutes, "minute", false);
    hours = this.inPlural(hours, "hour", false);
    days = this.inPlural(days, "day", false);
    return [days, hours, minutes].filter(el => el).join(", ");
  }

  // Formats the input number to specification. (5, "second", true) => "05 seconds"
  static inPlural(number, unit, withLeadingZero) {
    let value = withLeadingZero ? this.withLeadingZero(number) : number.toString();
    return number > 1 ? value + " " + unit + "s" : number > 0 ? value + " " + unit : null;
  }

  // Formats a positive number to at least two digits by prepending with "0"s
  static withLeadingZero(number) {
    return number >= 10 ? number.toString() : number > 0 ? "0" + number : "00";
  }

  static title(raw) {
    let replaced = raw.replace(/_/g, " ");
    return this.pascal(replaced);
  }

  static pascal(input) {
    let re = new RegExp(/\S*/, "g");
    return input.replace(re, this.singleWordPascal);
  }

  static singleWordPascal(match) {
    if (!match || match.length === 0) return match;
    let head = match[0].toUpperCase();
    let tail = match.length > 1 ? match.slice(1).toLowerCase() : "";
    return head + tail;
  }
}
