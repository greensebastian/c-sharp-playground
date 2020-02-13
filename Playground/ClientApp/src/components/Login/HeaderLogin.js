import React, { Component } from 'react';
import { connect } from 'react-redux';
import actions from '../../redux/actions';
import './Login.scss';

class HeaderLoginComponent extends Component {
  render() {
    return (
      <div className="header-login-wrapper">
        {this.props.loggedIn ? <p>{this.props.username}</p> : this.button()}
      </div>
    );
  }

  button() {
    return (<button onClick={() => this.props.dispatch(actions.loginShowModalUpdate(true))} className="header-login-button">Log in | Register</button>);
  }
}

function mapStateToProps(state) {
  return state.login;
}

const HeaderLogin = connect(mapStateToProps)(HeaderLoginComponent);
export default HeaderLogin;