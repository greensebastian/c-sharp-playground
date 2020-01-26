import React, { Component } from 'react';
import { DonutDistribution } from '../DonutDistribution/DonutDistribution';
import { Formatting } from '../../helpers/Formatting';

export class Distribution extends Component {

  render() {
    return this.props.data ? (this.renderPieCharts()) : <p>This component shows some distributions when data is loaded.</p>
  }

  renderPieCharts() {
    let activitiesByDistance = this.props.data.activitySegmentResults.Distance;
    let activitiesByTime = this.props.data.activitySegmentResults.Time;
    let placesByCount = this.props.data.placeVisitResults.Count;
    let placesByTime = this.props.data.placeVisitResults.Time;

    let data = [
      {
        title: "Travel methods by distance",
        totalCountText: "Total distance: " + Formatting.metersToKilometers(activitiesByDistance.totalCount),
        data: activitiesByDistance.dataSet,
        totalCount: activitiesByDistance.totalCount,
        conversion: (value) => Formatting.roundToDecimals(value / 1000, 2)
      },
      {
        title: "Travel methods by time",
        totalCountText: "Total time: " + Formatting.millisecondsToMinutes(activitiesByTime.totalCount),
        data: activitiesByTime.dataSet,
        totalCount: activitiesByTime.totalCount,
        conversion: (value) => Formatting.roundToDecimals(value / 1000 / 60, 0)
      },
      {
        title: "Places by count",
        totalCountText: "Total visits: " + Formatting.flat(placesByCount.totalCount),
        data: placesByCount.dataSet,
        totalCount: placesByCount.totalCount,
        conversion: (value) => value
      },
      {
        title: "Places by time",
        totalCountText: "Total time: " + Formatting.millisecondsToMinutes(placesByTime.totalCount),
        data: placesByTime.dataSet,
        totalCount: placesByTime.totalCount,
        conversion: (value) => Formatting.roundToDecimals(value / 1000 / 60, 0)
      }
    ];

    let siblingMargin = "1rem";

    let style = {
      marginLeft: "-" + siblingMargin,
      marginRight: "-" + siblingMargin
    };
    return (

      <div style={style}>
        <div className="d-flex flex-row flex-wrap justify-content-between w-100">
          {data.filter(this.hasData).map((dataPoint, index) => {
            return (
              <DonutDistribution
                key={index}
                title={dataPoint.title}
                totalCountText={dataPoint.totalCountText}
                data={dataPoint.data}
                totalCount={dataPoint.totalCount}
                conversion={dataPoint.conversion}
                siblingMargin={siblingMargin}
              />
            )
          })}
        </div>
      </div>
    );
  }

  hasData(dataPoint) {
    return dataPoint.data && dataPoint.data.length > 0;
  }
}