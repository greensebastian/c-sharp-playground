import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Test } from './components/Test';
import { Home } from './components/Home';
import { Counter } from './components/Counter';
import { StationInfo } from './components/StationInfo';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/station' component={StationInfo} />
        <Route path='/test' component={Test} />
      </Layout>
    );
  }
}
