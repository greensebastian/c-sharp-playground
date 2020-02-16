import React, { Component } from 'react';
import { PATHS } from '../../resources/Constants';
import { COLORS } from '../../resources/Colors';
import { SyncLoader } from "react-spinners";

export class StationInfo extends Component {
  static displayName = StationInfo.name;

  constructor(props) {
    super(props);
    this.state = { stations: [], loading: true };
  }

  componentDidMount() {
    this.populateStationData();
  }

  static renderStationsTable(stations) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Namn</th>
            <th>Linjetyp</th>
            <th>Kör mot</th>
            <th>Tågnummer</th>
            <th>Avgångstid</th>
          </tr>
        </thead>
        <tbody>
          {stations.map((station, index) =>
            <tr key={index}>
              <td>{station.name}</td>
              <td>{station.lineTypeName}</td>
              <td>{station.towards}</td>
              <td>{station.no}</td>
              <td>{StationInfo.formatDate(station.journeyDateTime)}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : StationInfo.renderStationsTable(this.state.stations);

    return (
      <div>
        <h2 id="tabelLabel" >Station information</h2>
        <p>This component shows public transportation leaving from Lund C.</p>
        <SyncLoader
          size={30}
          color={COLORS.SECONDARY.FIRST_DARK}
          loading={this.state.loading}
        />
        {contents}
      </div>
    );
  }

  async populateStationData() {
    const stationId = 81216;
    const response = await fetch(PATHS.SEARCH_STATION + '?stationId=' + stationId);
    const data = await response.json();
    this.setState({ stations: data.lines, loading: false });
  }

  static formatDate(rawDate) {
    let date = new Date(rawDate);
    return date.toLocaleString("sv-SE")
  }
}
