import React, { Component } from 'react';
import { PATHS } from '../../resources/Constants';
import { COLORS } from '../../resources/Colors';
import { DelayCard } from './DelayCard';
import { SyncLoader } from "react-spinners";

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
        <h3>Resor från {state.startPoint} till {state.endPoint}</h3>
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
        <h2 id="tabelLabel" >Delay information</h2>
        <p>This component retrieves and displays real time delay data.</p>
        <SyncLoader
          size={30}
          color={COLORS.SECONDARY.FIRST_DARK}
          loading={this.state.loading}
        />
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
