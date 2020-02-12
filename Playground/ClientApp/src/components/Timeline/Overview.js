import React, { Component } from 'react';

export class Overview extends Component {

  render() {
    return this.props.data ? (this.renderContent()) : <p>This component shows an overview when data is loaded.</p>
  }

  renderContent() {
    return (
      <div>
        <h2>Overview</h2>
        <p>
          This view shows an overview of the location data from your timeline information.
        </p>
        {this.renderOverview()}
      </div>
    );
  }

  renderOverview() {
    //let activitiesByDistance = this.props.data.activitySegmentResults.Distance;
    //let activitiesByTime = this.props.data.activitySegmentResults.Time;
    //let placesByCount = this.props.data.placeVisitResults.Count;
    //let placesByTime = this.props.data.placeVisitResults.Time;

    let home = this.props.data.placeVisitResults.Home;
    let work = this.props.data.placeVisitResults.Work;
    let daysAtWork = this.props.data.placeVisitResults.DaysAtWork;

    return (
      <div>
        <p>According to your timeline data, your worked at {work} and lived at {home}. You also spent {daysAtWork} days at work during the specified period.</p>
      </div>
    );
  }

  hasData(dataPoint) {
    return dataPoint.data && dataPoint.data.length > 0;
  }
}