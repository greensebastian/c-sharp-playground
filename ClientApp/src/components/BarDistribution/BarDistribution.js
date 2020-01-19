import React, { Component } from 'react';
import './BarDistribution.css';

export class BarDistribution extends Component {
  static displayName = BarDistribution.name;

  render() {
    let limit = this.props.limit;
    let data = this.props.data.slice(0, limit);
    let title = this.props.title;
    let lineColor = this.props.lineColor;
    let totalCountText = this.props.totalCountText;
    return (
      <div className="d-flex flex-column mb-3 w-100">
        <h5>{title}</h5>
        <h6>{totalCountText}</h6>
        {data.map(data => BarDistribution.formattedLine(data, lineColor, this.props.unitFormat))}
      </div>
    );
  }

  static formattedLine(line, lineColor, unitConversion) {
    const lineStyle = {
      backgroundColor: lineColor,
      width: line.normalizedFraction * 100 + "%"
    }
    return (
      <div className="bar-container w-100" key={line.key}>
        <p className="mb-1 pb-0">{line.name}, {unitConversion(line.count)}</p>
        <div className="d-flex flex-row justify-content-start align-items-center mb-2 w-100">
          <div className="bar-line m-0 p-0 d-flex flex-grow-0" style={lineStyle}>
          </div>
        </div>
      </div>
    );
  }

  formattedTime(date) {
    let dateObj = new Date(date);
    let hours = dateObj.getHours();
    hours = hours < 10 ? "0" + hours : hours.toString();
    let minutes = dateObj.getMinutes();
    minutes = minutes < 10 ? "0" + minutes : minutes.toString();
    return hours + ":" + minutes;
  }

  static millisecondsToMinutes(ms) {
    return BarDistribution.roundToDecimals(ms / 1000 / 60, 0) + " minutes";
  }

  static metersToKilometers(m) {
    return BarDistribution.roundToDecimals(m / 1000, 2) + " km";
  }

  static flat(unit) {
    return unit + " times";
  }

  // Kinda ugly but useful and simple
  static roundToDecimals(value, decimals) {
    let multiplier = Math.pow(10, decimals);
    return Math.round(multiplier * value) / multiplier;
  }
}
