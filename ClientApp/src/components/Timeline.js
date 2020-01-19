import React, { Component } from 'react';
import { PATHS } from '../resources/Constants';
import { COLORS } from '../resources/Colors';
import { BarDistribution } from './BarDistribution/BarDistribution';

export class Timeline extends Component {
  static displayName = Timeline.name;

  constructor(props) {
    super(props);
    this.state = {
      selectedFile: null,
      working: false,
      processedData: null,
      error: null,
      showDemo: false
    }
  }

  render() {

    return (
      <div>
        <h1 id="tabelLabel">Google maps timeline visualization</h1>
        <p>
          This page analyzes and shows information about the google maps timeline data you provide. Data is provided through visiting <a href="https://takeout.google.com/settings/takeout/custom/location_history" target="_blank" rel="noopener noreferrer">takeout.google.com</a> and retrieving a JSON dump of your timeline data. Unpack the location history and select a Semantic Location History file from:<br/>
          <b>/Location History/Semantic History Location/<i>year</i>/<i>year-month</i>.json</b>
        </p>
        <div>
          <label>Select your file:</label><br />
          <input type="file" className="mb-2" name="files" accept="*.json" onChange={this.onSelectedFileChange} /><br />
          <button type="button" className="mb-2" onClick={this.onClickUpload} disabled={!this.state.selectedFile && !this.state.showDemo}>Upload</button><br />
          <div className="d-flex flex-row align-items-center mb-2">
            <input type="checkbox" className="mr-2" name="showDemo" onChange={this.onClickDemoUpload} />Include demo file
          </div>
        </div>
        {this.contentSection()}
      </div>
    );
  }

  contentSection() {
    if (this.state.error) {
      return <p>Something went wrong...</p>
    }
    else if (this.state.processedData !== null) {
      /*return (
        <div>
          <p>Found {this.state.processedData.resultCount.toString()} timeline records, {this.state.processedData.results.filter(this.isPlaceVisit).length.toString()} of which are place visits!</p>
          {this.renderVisits(this.state.processedData.results)}
        </div>
      );*/

      let activitiesByDistance = this.state.processedData.processedResults.activitySegmentResults.Distance;
      let activitiesByDistanceFormatting = BarDistribution.metersToKilometers;

      let activitiesByTime = this.state.processedData.processedResults.activitySegmentResults.Time;
      let activitiesByTimeFormatting = BarDistribution.millisecondsToMinutes;

      let placesByTime = this.state.processedData.processedResults.placeVisitResults.Time;
      let placesByTimeFormatting = BarDistribution.millisecondsToMinutes;

      let placesByCount = this.state.processedData.processedResults.placeVisitResults.Count;
      let placesByCountFormatting = BarDistribution.flat;
      return (
        <div>
          <BarDistribution
            data={activitiesByDistance.dataSet}
            title={"Travel methods by distance"}
            lineColor={COLORS.SECONDARY}
            totalCountText={"Total distance: " + activitiesByDistanceFormatting(activitiesByDistance.totalCount)}
            unitFormat={activitiesByDistanceFormatting}
            limit={5}
          />
          <BarDistribution
            data={activitiesByTime.dataSet}
            title={"Travel methods by time"}
            lineColor={COLORS.SECONDARY}
            totalCountText={"Total time: " + activitiesByTimeFormatting(activitiesByTime.totalCount)}
            unitFormat={activitiesByTimeFormatting}
            limit={5}
          />
          <BarDistribution
            data={placesByTime.dataSet}
            title={"Places visited by time"}
            lineColor={COLORS.SECONDARY}
            totalCountText={"Total time: " + placesByTimeFormatting(placesByTime.totalCount)}
            unitFormat={placesByTimeFormatting}
            limit={10}
          />
          <BarDistribution
            data={placesByCount.dataSet}
            title={"Places visited by count"}
            lineColor={COLORS.SECONDARY}
            totalCountText={"Total number of visits: " + placesByCountFormatting(placesByCount.totalCount)}
            unitFormat={placesByCountFormatting}
            limit={10}
          />
        </div>
      );
    }
    else return "";
  }

  renderVisits(visits) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Visit #</th>
            <th>Place</th>
            <th>Arrived at</th>
            <th>Stayed for</th>
          </tr>
        </thead>
        <tbody>
          {visits.filter(this.isPlaceVisit).map((visit, index) => this.renderPlaceVisit(visit, index))}
        </tbody>
      </table>
    );
  }

  isPlaceVisit(result) {
    if (result.placeVisit) return true;
    else return false;
  }

  // Renders a table row from the given object and index
  renderPlaceVisit(visit, index) {
    if (!this.isPlaceVisit(visit)) return;

    let placeVisit = visit.placeVisit;

    let location = placeVisit.location;
    let duration = parseInt(placeVisit.duration.endTimestampMs - placeVisit.duration.startTimestampMs);

    let date = parseInt(placeVisit.duration.startTimestampMs);

    return (
      <tr key={index}>
        <td>{parseInt(index) + 1}</td>
        <td>{location.name}</td>
        <td>{this.formatDate(date)}</td>
        <td>{this.formatDuration(duration)}</td>
      </tr>
    );
  }

  formatDate(rawDate) {
    let date = new Date(rawDate);
    return date.toLocaleDateString() + " " + this.withLeadingZero(date.getHours()) + ":" + this.withLeadingZero(date.getMinutes());
  }

  // Formats a duration in milliseconds to days/hours/minutes, (10 mil) => "2 hours, 46 minutes"
  formatDuration(rawDuration) {
    let minutes = Math.floor(rawDuration / 1000 / 60) % 60;
    let hours = Math.floor(rawDuration / 1000 / 60 / 60) % 24;
    let days = Math.floor(rawDuration / 1000 / 60 / 60 / 24);
    minutes = this.inPlural(minutes, "minute", false);
    hours = this.inPlural(hours, "hour", false);
    days = this.inPlural(days, "day", false);
    return [days, hours, minutes].filter(el => el).join(", ");
  }

  // Formats the input number to specification. (5, "second", true) => "05 seconds"
  inPlural(number, unit, withLeadingZero) {
    let value = withLeadingZero ? this.withLeadingZero(number) : number.toString();
    return number > 1 ? value + " " + unit + "s" : number > 0 ? value + " " + unit : null;
  }

  withLeadingZero(number) {
    return number >= 10 ? number.toString() : number > 0 ? "0" + number : "00";
  }

  // Toggle inclusion of the demo file
  onClickDemoUpload = (event) => {
    this.setState({ showDemo: !this.state.showDemo });
  }

  // Calls the server and updates state with response data
  onClickUpload = (event) => {
    if (this.state.working || (!this.state.showDemo && !this.state.selectedFile)) return;
    const data = new FormData();

    if (this.state.selectedFile) {
      data.append("files", this.state.selectedFile, this.state.selectedFile.name);
    }

    data.append("showDemo", this.state.showDemo);

    this.setState({ working: true })

    // POST to server api with parameters from state
    // Handles updating of the state asynchronously through promises
    fetch(PATHS.TIMELINE_POST, {
      method: "POST",
      body: data
    }).then((response) => response.json())
      .then((result) => {
        if (!result || result.error) throw result;
        this.setState({ working: false, processedData: result, error: null });
      })
      .catch((error) => {
        this.setState({ working: false, error: error });
      });
  }

  onSelectedFileChange = (event) => {
    this.setState({
      selectedFile: event.target.files[0]
    });
  }
}
