import React, { Component } from "react";
import { Form, FormGroup, Input, Label, FormFeedback } from "reactstrap";
import { connect } from "react-redux";
import { SyncLoader } from "react-spinners";
import actions from "../../../redux/actions";
import { COLORS } from "../../../resources/Colors";
import { postLogin, getLoginState } from "../LoginHandler";
import "../Login.scss";

class LoginTabComponent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      submitEnabled: true,
      username: "",
      password: "",
      buttonMessage: "",
      modalCloseTimer: undefined
    };
  }

  formData() {
    var data = new FormData();
    data.append("Username", this.state.username);
    data.append("Password", this.state.password);
    return data;
  }

  jsonData() {
    return JSON.stringify(this.state);
  }

  render() {
    if (!this.props.show) return "";
    return (
      <Form>
        <FormGroup>
          <Label for="username">Username</Label>
          <Input
            type="text"
            name="username"
            id="username"
            placeholder="Username"
            value={this.state.username}
            invalid={!validUsername(this.state.username)}
            onChange={e => this.setState({ username: e.target.value })}
          />
          <FormFeedback>Username cannot be empty</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            type="password"
            name="password"
            id="password"
            placeholder="Password"
            value={this.state.password}
            invalid={!validPassword(this.state.password)}
            onChange={e => this.setState({ password: e.target.value })}
          />
          <FormFeedback>Password cannot be empty</FormFeedback>
        </FormGroup>
        <span className="form-submit">
          <button
            enabled={this.state.submitEnabled.toString()}
            onClick={this.handleSubmit.bind(this)}
          >
            Login
          </button>
          <SyncLoader
            size={15}
            color={COLORS.SECONDARY.FIRST_DARK}
            loading={this.state.loading}
          />
          <p>{this.state.buttonMessage}</p>
        </span>
      </Form>
    );
  }

  async handleSubmit(event) {
    event.preventDefault();
    if (!this.state.submitEnabled) return;
    let data = this.jsonData();
    this.setState({ loading: true, submitEnabled: false });
    let response = await postLogin(data);
    this.setState({ loading: false, submitEnabled: true });
    if (response.statusCode) {
      switch (response.statusCode) {
        case 200:
          this.setState({
            buttonMessage: "Success! Logged in as " + response.username
          });
          this.updateLoginState(response);
          break;
        case 400:
          this.setState({
            buttonMessage: "Error! Some fields have invalid values"
          });
          break;
        case 401:
          this.setState({
            buttonMessage:
              "Error! Username and password combination does not exist"
          });
          break;
        case 403:
          this.setState({
            buttonMessage:
              "Error! Username and password combination does not exist"
          });
          break;
        default:
          this.setState({
            buttonMessage: "Error! An unexpected error occured"
          });
          break;
      }
    } else {
      this.setState({ buttonMessage: "Error! An unexpected error occured" });
    }
  }

  updateLoginState(serviceResponse) {
    const loginState = getLoginState(serviceResponse);
    this.props.dispatch(actions.loginStateUpdate(loginState));
  }
}

function validUsername(username) {
  return username.length > 0;
}

function validPassword(password) {
  return password.length > 0;
}

function mapStateToProps(state) {
  return { loginState: state.login };
}

const LoginTab = connect(mapStateToProps)(LoginTabComponent);
export default LoginTab;
