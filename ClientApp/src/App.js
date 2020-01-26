import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Test } from './components/Test';
import { Home } from './components/Home';
import { StationInfo } from './components/StationInfo';
import { DelayStatus } from './components/DelayStatus';
import { Timeline } from './components/Timeline/Timeline';

import './custom.css'
import './reset.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/delay' component={DelayStatus} />
        <Route path='/timeline' component={Timeline} />
        <Route path='/station' component={StationInfo} />
        <Route path='/test' component={Test} />
      </Layout>
    );
  }
}
