import React, { Component } from 'react';
import { Route } from 'react-router';
import { Provider } from 'react-redux';
import createReduxStore from './redux';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Timeline } from './components/Timeline/Timeline';
import { Trains } from './components/Trains/Trains';
import LoginLayout from './components/Login/Modal/LoginLayout';

import './custom.scss';
import './reset.scss';

const store = createReduxStore();

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Provider store={store}>
        <Layout>
          <Route exact path='/' component={Home} />
          <Route path='/timeline' component={Timeline} />
          <Route path='/trains' component={Trains} />

          <LoginLayout />
        </Layout>
      </Provider>
    );
  }
}
