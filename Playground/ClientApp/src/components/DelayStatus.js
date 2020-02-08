import React, { Component } from 'react';
import { PATHS } from '../resources/Constants';
import { DelayCard } from './DelayCard';

export class DelayStatus extends Component {
  static displayName = DelayStatus.name;

  constructor(props) {
    super(props);
    this.state = { startPoint: "", endPoint: "", journeys: [], loading: true };
  }

  componentDidMount() {
    this.populateDelayData();
  }

  static renderDelayComponents(state) {
    let journeys = state.journeys;
    return (
      <div>
        <h2>Resor från {state.startPoint} till {state.endPoint}</h2>
        {journeys.map((journey, index) =>
          < DelayCard key={index} journey={journey}/>
        )}
      </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : DelayStatus.renderDelayComponents(this.state);

    return (
      <div>
        <h1 id="tabelLabel" >Delay information</h1>
        <p>This component retrieves and displays real time delay data.</p>
        {contents}
      </div>
    );
  }

  async populateDelayData() {
    const fromPoint = "Lund C";
    const toPoint = "Köpenhamn Österport";
    const date = new Date(Date.now() - 1000 * 60 * 60);
    const dateString = date.toISOString();
    const response = await fetch(PATHS.DELAY_STATUS + '?startPoint=' + fromPoint + "&endPoint=" + toPoint + "&dateTime=" + dateString + "&numberResults=" + 15);
    const data = await response.json();
    this.setState({ startPoint: data.startPoint, endPoint: data.endPoint, journeys: data.journeys, loading: false });
  }

  static formatDate(rawDate) {
    let date = new Date(rawDate);
    return date.toLocaleString("sv-SE")
  }
}