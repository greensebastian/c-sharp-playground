import React, { Component } from "react";
import { connect } from "react-redux";
import { MdClose } from "react-icons/md";
import actions from "../../../redux/actions";
import LoginTab from "./LoginTab";
import RegisterTab from "./RegisterTab";
import "../Login.scss";
import { FormGroup, Label, Input } from "reactstrap";
import { postLogout, deleteMe, getLoginState } from "../LoginHandler";

const TABS = {
  LOGIN: 0,
  REGISTER: 1
};

const selectedClassName = "selected";

class LoginLayoutComponent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      activeTab: TABS.LOGIN,
      loginClassName: selectedClassName,
      registerClassName: "",
      showDeleteDetails: false,
      password: ""
    };
  }

  render() {
    return this.props.login.showModal ? this.renderContent() : "";
  }

  renderContent() {
    return (
      <div onMouseDown={this.closeModal.bind(this)} className="login-blackout">
        <div onMouseDown={event => event.stopPropagation()} className="login-modal">
          <header>
            <h3>Your profile</h3>
            <p>Save your information between sessions by logging in.</p>
          </header>
          <MdClose
            onClick={this.closeModal.bind(this)}
            size="50"
            className="login-modal-close"
          />
          {this.props.login.loggedIn
            ? this.profileView()
            : this.loginOrRegisterView()}
        </div>
      </div>
    );
  }

  profileView() {
    return (
      <div className="content-section">
        <table className="table">
          <thead>
            <tr>
              <th>Attribute</th>
              <th>Value</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>Username</td>
              <td>{this.props.login.username}</td>
            </tr>
            <tr>
              <td>Email</td>
              <td>{this.props.login.email}</td>
            </tr>
            <tr>
              <td>Activity Segments</td>
              <td>{this.props.login.activitySegmentCount}</td>
            </tr>
            <tr>
              <td>Place Visits</td>
              <td>{this.props.login.placeVisitCount}</td>
            </tr>
          </tbody>
        </table>
        <div className="profile-actions">
          <button onClick={this.handleLogout.bind(this)}>Logout</button>
          {this.state.showDeleteDetails ? this.deleteDetails() : ""}
          <button onClick={this.handleDelete.bind(this)}>Delete me</button>
        </div>
      </div>
    );
  }

  async handleLogout(event) {
    event.preventDefault();
    var response = await postLogout();
    if (response.statusCode === 200) {
      this.resetShowDetails();
      this.updateLoginState();
    } else {
      alert("Something went wrong when logging out");
    }
  }

  async handleDelete(event) {
    event.preventDefault();
    if (!this.state.showDeleteDetails) {
      this.setState({ showDeleteDetails: true });
      return;
    }
    var response = await deleteMe(
      JSON.stringify({ Password: this.state.password })
    );
    if (response.statusCode === 200) {
      this.resetShowDetails();
      this.updateLoginState();
      alert("Successfully deleted user, your are now logged out");
    } else {
      alert("Something went wrong when deleting user");
    }
  }

  updateLoginState() {
    const loginState = getLoginState();
    this.props.dispatch(actions.loginStateUpdate(loginState));
  }

  deleteDetails() {
    return (
      <section>
        <p className="warning">Warning: You cannot undo this action!</p>
        <FormGroup>
          <Label for="password">Enter password to confirm</Label>
          <Input
            type="password"
            name="password"
            id="password"
            placeholder="Password"
            value={this.state.password}
            onChange={e => this.setState({ password: e.target.value })}
          />
        </FormGroup>
      </section>
    );
  }

  loginOrRegisterView() {
    return (
      <div className="content-section">
        <nav className="action-tabs">
          <button
            onClick={() => this.setActiveTab(TABS.LOGIN)}
            className={this.state.loginClassName}
          >
            Login
          </button>
          <button
            onClick={() => this.setActiveTab(TABS.REGISTER)}
            className={this.state.registerClassName}
          >
            Register
          </button>
        </nav>
        <LoginTab show={this.state.activeTab === TABS.LOGIN} />
        <RegisterTab show={this.state.activeTab === TABS.REGISTER} />
      </div>
    );
  }

  setActiveTab(tabIndex) {
    this.resetShowDetails();
    this.setState({
      activeTab: tabIndex,
      loginClassName: tabIndex === TABS.LOGIN ? selectedClassName : "",
      registerClassName: tabIndex === TABS.REGISTER ? selectedClassName : ""
    });
  }

  navButton() {
    return <button className="header-login-button">Log in | Register</button>;
  }

  closeModal() {
    this.props.dispatch(actions.loginShowModalUpdate(false));
  }

  componentDidUpdate(prevProps, _) {
    if (prevProps.login && !prevProps.login.showModal) {
      this.resetShowDetails();
    }
  }

  resetShowDetails(){
    this.setState({ showDeleteDetails: false });
  }
}

function mapStateToProps(state) {
  return { login: state.login };
}

const LoginLayout = connect(mapStateToProps)(LoginLayoutComponent);
export default LoginLayout;
