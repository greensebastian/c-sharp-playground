import React, { Component } from 'react';
import { MdDirectionsBus, MdAccessTime, MdCancel } from 'react-icons/md';

export class DelayCard extends Component {
  static displayName = DelayCard.name;

  render() {
    let journey = this.props.journey;
    return (
      <div className="card d-flex flex-column mb-3">
        <div className="card-body">
          <div className="d-flex flex-row justify-content-start align-items-center mb-2">
            <h5 className="card-title m-0">{journey.departure.name} till {journey.arrival.name}, {this.formattedTime(new Date(journey.departure.time))}</h5>
            {this.getIcons(journey)}
          </div>
          <table className="table">
            <tbody>
              {this.formattedPoint(journey.departure)}
              {this.formattedPoint(journey.arrival)}
            </tbody>
          </table>
        </div>
      </div>
    );
  }

  getIcons(journey) {
    let delay = Math.max(journey.departure.deviation, journey.arrival.deviation);
    let delayColor = this.delayColor(delay);
    let className = "m-2";
    let size = "2rem";
    return (
      <div className="d-flex flex-row justify-content-start">
        {journey.delayed ? <MdAccessTime className={className} color={delayColor} size={size} /> : ""}
        {journey.cancelled ? <MdCancel className={className} color="red" size={size} /> : ""}
        {journey.replacementBuses ? <MdDirectionsBus className={className} color="red" size={size} /> : ""}
      </div>
    );
  }

  delayColor(delay) {
    let colorMin = 0xffb100;
    let colorMax = 0xff0000;
    const maxDelay = 20.0;
    let delayFraction = delay / maxDelay;
    // Limit fraction to [0, 1] 
    delayFraction = delayFraction < 0 ? 0 : delayFraction > 1 ? 1 : delayFraction;
    let delayColor = colorMin + delayFraction * (colorMax - colorMin);
    return "#" + Math.round(delayColor).toString(16);
  }

  formattedPoint(point) {
    let dateObj = new Date(point.time);
    dateObj.setMinutes(dateObj.getMinutes() + point.deviation);
    return (
      <tr>
        <td className="pl-0">{point.name}</td>
        <td>{this.formattedTime(dateObj)}</td>
        <td>{point.deviation} minuter</td>
      </tr>
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
}
