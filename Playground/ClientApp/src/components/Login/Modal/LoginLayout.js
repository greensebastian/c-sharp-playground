import React, { Component } from 'react';
import { connect } from 'react-redux';
import { MdClose } from 'react-icons/md';
import actions from '../../../redux/actions';
import LoginTab from './LoginTab';
import RegisterTab from './RegisterTab';
import '../Login.scss';

const TABS = {
  LOGIN: 0,
  REGISTER: 1
}

const selectedClassName = "selected";

class LoginLayoutComponent extends Component {

  constructor(props){
    super(props);
    this.state = {
      activeTab: TABS.LOGIN,
      loginClassName: selectedClassName,
      registerClassName: ""
    };
  }

  render() {
    return this.props.show ? this.renderContent() : "";
  }

  renderContent() {
    return (
      <div onClick={this.closeModal.bind(this)} className="login-blackout">
        <div onClick={(event) => event.stopPropagation()} className="login-modal">
          <header>
            <h3>
              Login or register
            </h3>
            <p>Only visual as of now, not yet connected with backend.</p>
          </header>
          <MdClose onClick={this.closeModal.bind(this)} size="50" className="login-modal-close" />
          <div className="content-section">
          <nav className="action-tabs">
            <button onClick={() => this.setActiveTab(TABS.LOGIN)} className={this.state.loginClassName}>
              Login
            </button>
            <button onClick={() => this.setActiveTab(TABS.REGISTER)} className={this.state.registerClassName}>
              Register
            </button>
          </nav>
          {this.state.activeTab === TABS.LOGIN ? <LoginTab /> : <RegisterTab />}
          </div>
        </div>
      </div>
    );
  }

  setActiveTab(tabIndex){
    this.setState({
      activeTab: tabIndex,
      loginClassName: tabIndex === TABS.LOGIN ? selectedClassName : "",
      registerClassName: tabIndex === TABS.REGISTER ? selectedClassName : ""
    })
    
  }

  navButton() {
    return (<button className="header-login-button">Log in | Register</button>);
  }

  closeModal() {
    this.props.dispatch(actions.loginShowModalUpdate(false));
  }
}

function mapStateToProps(state) {
  return { show: state.login.showModal };
}

const LoginLayout = connect(mapStateToProps)(LoginLayoutComponent);
export default LoginLayout;