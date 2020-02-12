import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Timeline } from './components/Timeline/Timeline';
import { Trains } from './components/Trains/Trains';

import './custom.css'
import './reset.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/timeline' component={Timeline} />
        <Route path='/trains' component={Trains} />
      </Layout>
    );
  }
}
