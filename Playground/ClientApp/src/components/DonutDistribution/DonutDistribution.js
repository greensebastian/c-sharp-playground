import React, { Component } from 'react';
import './DonutDistribution.css';
import { COLORS } from '../../resources/Colors';
import ReactMinimalPieChart from 'react-minimal-pie-chart';
import { Formatting } from '../../helpers/Formatting';

export class DonutDistribution extends Component {
  static displayName = DonutDistribution.name;
  static COLORS = [
    COLORS.SECONDARY.FIRST,
    COLORS.PRIMARY.FIRST,
    COLORS.SECONDARY.FIRST_DARK,
    COLORS.PRIMARY.FIRST_DARK,
    COLORS.SECONDARY.SECOND,
    COLORS.SECONDARY.SECOND_DARK,
    COLORS.PRIMARY.SECOND,
    COLORS.PRIMARY.SECOND_DARK,
  ];

  render() {
    let title = this.props.title;
    let totalCountText = this.props.totalCountText;
    let style = {
      marginLeft: this.props.siblingMargin,
      marginRight: this.props.siblingMargin
    }
    return (
      <div className="d-flex flex-grow-1 flex-column mb-3 donut-distribution" style={style}>
        <h5 className="nowrap">{title}</h5>
        <h6>{totalCountText}</h6>
        {this.formattedPieChart()}
        {this.caption()}
      </div>
    );
  }

  formattedPieChart() {
    return (
      <ReactMinimalPieChart
        style={{
          marginBottom: "1rem"
        }}
        animate
        animationDuration={500}
        animationEasing="ease-out"
        cx={50}
        cy={50}
        data={this.toPieChartDataFormat()}
        label
        labelPosition={75}
        labelStyle={{
          fontFamily: 'sans-serif',
          fontSize: '5px'
        }}
        lengthAngle={360}
        lineWidth={15}
        onClick={undefined}
        onMouseOut={undefined}
        onMouseOver={undefined}
        paddingAngle={1}
        radius={50}
        rounded={false}
        startAngle={180}
        viewBoxSize={[
          100,
          100
        ]}
      />
    );
  }

  caption() {
    return (
      <figcaption className="d-flex flex-row flex-wrap justify-content-center">{this.toPieChartDataFormat().map((data, index) => {
        return (Formatting.captionSpan(data.color, data.title, index))
      })}</figcaption>
    );
  }

  toPieChartDataFormat() {
    let events = this.props.data;
    let conversion = this.props.conversion;
    let totalCount = this.props.totalCount;
    // Keep track of processed resources
    let accumulatedCount = 0;
    let lastUsedColorIndex = 0;
    // Map all data points onto a new format
    let data = events.slice(0, 7).filter(this.shouldInclude).map((event, index) => {
      let count = event.count;
      lastUsedColorIndex = index;
      accumulatedCount += count;
      return {
        color: DonutDistribution.COLORS[index],
        title: this.format(event.name),
        value: conversion(count)
      };
    });
    // Check if an "Other" section should be added
    if (totalCount > accumulatedCount) {
      data[data.length] = {
        color: DonutDistribution.COLORS[lastUsedColorIndex + 1],
        title: "Other",
        value: conversion(totalCount - accumulatedCount)
      }
    };
    return data;
  }

  // Hide data points with less than specified fraction of total
  shouldInclude(event) {
    return event.normalizedFraction > 0.1;
  }

  format(text) {
    return this.props.shouldFormat ? Formatting.title(text) : text;
  }
}
