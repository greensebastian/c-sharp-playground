import React, { Component } from 'react';
import { Dropdown, DropdownButton } from 'react-bootstrap';
import { PATHS } from '../../resources/Constants';
import { Distribution } from './Distribution';

export class Timeline extends Component {
  static displayName = Timeline.name;

  static VIEWS = {
    DISTRIBUTION: "Distribution"
  };

  constructor(props) {
    super(props);
    this.state = {
      selectedFile: null,
      working: false,
      processedData: null,
      error: null,
      showDemo: false,
      view: Timeline.VIEWS.DISTRIBUTION
    }
  }

  render() {

    return (
      <div>
        <h1 id="tabelLabel">Google maps timeline visualization</h1>
        <p>
          This page analyzes and shows information about the google maps timeline data you provide. Data is provided through visiting <a href="https://takeout.google.com/settings/takeout/custom/location_history" target="_blank" rel="noopener noreferrer">takeout.google.com</a> and retrieving a JSON dump of your timeline data. Unpack the location history and select a Semantic Location History file from:<br />
          <b>/Location History/Semantic History Location/<i>year</i>/<i>year-month</i>.json</b>
        </p>
        <div>
          <label>Select your file (or include the demo file):</label><br />
          <input type="file" className="mb-2" name="files" accept="*.json" onChange={this.onSelectedFileChange} /><br />
          <button type="button" className="mb-2" onClick={this.onClickUpload} disabled={!this.state.selectedFile && !this.state.showDemo}>Upload</button><br />
          <div className="d-flex flex-row align-items-center mb-2">
            <input id="use-demo" type="checkbox" className="mr-2" name="showDemo" onChange={this.onClickDemoUpload} /><label htmlFor="use-demo" className="m-0 p-0">Include demo file</label>
          </div>
          {this.dropdown()}
          {this.contentSection()}
        </div>
        {}
      </div>
    );
  }

  dropdown() {
    return this.state.processedData ? (
      <Dropdown>
        <DropdownButton id="dropdown-basic-button" title={this.state.view}>
          <Dropdown.Item onClick={() => this.setView(Timeline.VIEWS.DISTRIBUTION)}>{Timeline.VIEWS.DISTRIBUTION}</Dropdown.Item>
        </DropdownButton>
      </Dropdown>
    ) : null
  }

  setView(view) {
    this.setState({ view: view })
  }

  contentSection() {
    if (this.state.error) {
      return <p>Something went wrong...</p>
    }
    else if (this.state.processedData) {
      return (this.state.view === Timeline.VIEWS.DISTRIBUTION) ? (<Distribution data={this.state.processedData.processedResults} />) : null;
    }
    else return "";
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
