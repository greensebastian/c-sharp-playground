import React, { Component } from 'react';
import { Dropdown, DropdownButton } from 'react-bootstrap';
import { StationInfo } from './StationInfo';
import { DelayStatus } from './DelayStatus';

export class Trains extends Component {
  static displayName = Trains.name;

  static VIEWS = {
    STATION_INFO: "Station info",
    DELAY_STATUS: "Delay status"
  };

  constructor(props) {
    super(props);
    this.state = {
      view: Trains.VIEWS.DELAY_STATUS
    }
  }

  render() {

    return (
      <div>
        <h1 id="tabelLabel">Public transport information</h1>
        <p>
          This page shows some information about public transport to and from Lund C.
        </p>
        <div>
          {this.dropdown()}
          {this.contentSection()}
        </div>
        {}
      </div>
    );
  }

  dropdown() {
    return (
      <div>
        <Dropdown>
          <DropdownButton id="dropdown-basic-button" title={this.state.view}>
            <Dropdown.Item onClick={() => this.setView(Trains.VIEWS.DELAY_STATUS)}>{Trains.VIEWS.DELAY_STATUS}</Dropdown.Item>
            <Dropdown.Item onClick={() => this.setView(Trains.VIEWS.STATION_INFO)}>{Trains.VIEWS.STATION_INFO}</Dropdown.Item>
          </DropdownButton>
        </Dropdown>
        <p>Access the different public transport views by changing the dropdown selection.</p>
      </div>
    );
  }

  setView(view) {
    this.setState({ view: view })
  }

  contentSection() {
    if (this.state.view === Trains.VIEWS.STATION_INFO) return <StationInfo />;
    else if (this.state.view === Trains.VIEWS.DELAY_STATUS) return <DelayStatus />;
    else return "";
  }
}
