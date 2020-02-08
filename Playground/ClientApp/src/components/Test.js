import React, { Component } from 'react';

export class Test extends Component {
  static displayName = Test.name;

  constructor(props) {
    super(props);
    this.state = { text: "" };
  }

  componentDidMount() {
    this.callServices();
  }

  render() {
    let contents = this.state.text;
    return (
      <div>
        {contents}
      </div>
    );
  }

  async callServices() {
    const response = await fetch('test');
    const data = await response.text();
    this.setState({ text: data });
  }
}
