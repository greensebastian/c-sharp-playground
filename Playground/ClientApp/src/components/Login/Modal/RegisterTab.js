import React, { Component } from "react";
import { Form, FormGroup, Input, Label, FormFeedback, FormText } from "reactstrap";
import { connect } from "react-redux";
import { SyncLoader } from "react-spinners";
import actions from "../../../redux/actions";
import { COLORS } from "../../../resources/Colors";
import { postRegister, getLoginState } from "../LoginHandler";
import "../Login.scss";

class RegisterTabComponent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      submitEnabled: true,
      username: "",
      email: "",
      password: "",
      repassword: "",
      registrationKey: "",
      buttonMessage: ""
    };
  }

  formData() {
    var data = new FormData();
    data.append("Username", this.state.username);
    data.append("Email", this.state.email);
    data.append("Password", this.state.password);
    data.append("RegistrationKey", this.state.registrationKey);
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
            autoComplete="username"
            value={this.state.username}
            invalid={!validUsername(this.state.username)}
            onChange={e => this.setState({ username: e.target.value })}
          />
          <FormFeedback>Username cannot be empty</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Label for="email">Email</Label>
          <Input
            type="email"
            name="email"
            id="email"
            placeholder="user@example.com"
            autoComplete="email"
            value={this.state.email}
            invalid={!validEmail(this.state.email)}
            onChange={e => this.setState({ email: e.target.value })}
          />
          <FormFeedback>An email must contain at least one @ sign</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            type="password"
            name="password"
            id="password"
            placeholder="Password"
            autoComplete="new-password"
            value={this.state.password}
            invalid={!validPassword(this.state.password)}
            onChange={e => this.setState({ password: e.target.value })}
          />
          <FormFeedback>The password must contain at least 4 characters</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Label for="retype-password">Enter Password Again</Label>
          <Input
            type="password"
            name="retype-password"
            id="retype-password"
            placeholder="Password"
            autoComplete="new-password"
            value={this.state.repassword}
            invalid={
              !passwordsEqual(this.state.password, this.state.repassword)
            }
            onChange={e => this.setState({ repassword: e.target.value })}
          />
          <FormFeedback>The passwords must match</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Label for="registrationKey">Registration Key</Label>
          <Input
            type="password"
            name="registrationKey"
            id="registrationKey"
            placeholder="Registration Key"
            autoComplete="nope"
            value={this.state.registrationKey}
            onChange={e => this.setState({ registrationKey: e.target.value })}
          />
          <FormText>You need a key to register, contact me if you want to register!</FormText>
        </FormGroup>
        <span className="form-submit">
          <button enabled={this.state.submitEnabled.toString()} onClick={this.handleSubmit.bind(this)}>Register</button>
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

  async handleSubmit(event){
    event.preventDefault();
    if (!this.state.submitEnabled) return;
    let data = this.jsonData();
    this.setState({loading: true, submitEnabled: false});
    let response = await postRegister(data);
    this.setState({loading: false, submitEnabled: true});
    if (response.statusCode){
      switch(response.statusCode){
        case 200:
          this.setState({buttonMessage: "Success! Logged in as " + response.username});
          this.updateLoginState(response);
          break;
        case 400:
          this.setState({buttonMessage: "Error! Registration details are not valid"});
          break;
        default:
          this.setState({buttonMessage: "Error! An unexpected error occured"});
          break;
      }
    }
    else {
      this.setState({buttonMessage: "Error! An unexpected error occured"});
    }
  }

  updateLoginState(serviceResponse){
    const loginState = getLoginState(serviceResponse);
    this.props.dispatch(actions.loginStateUpdate(loginState));
  }
}

function validUsername(username) {
  return username.length > 0;
}

function validEmail(email) {
  return email.length > 0 && email.indexOf("@") > 0;
}

function validPassword(password) {
  return password.length >= 4;
}

function passwordsEqual(p1, p2) {
  return p1 === p2;
}

function mapStateToProps(state) {
  return { loginState: state.login };
}

const RegisterTab = connect(mapStateToProps)(RegisterTabComponent);
export default RegisterTab;
