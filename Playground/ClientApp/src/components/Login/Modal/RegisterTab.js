import React, { Component } from "react";
import { Form, FormGroup, Input, Label } from "reactstrap";
import { connect } from "react-redux";
import { SyncLoader } from "react-spinners";
import actions from "../../../redux/actions";
import { COLORS } from "../../../resources/Colors";
import { PATHS } from "../../../resources/Constants";
import "../Login.scss";

class RegisterTabComponent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      username: "",
      email: "",
      password: "",
      repassword: "",
      registrationKey: "",
      serviceResponse: ""
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
        </FormGroup>
        <FormGroup>
          <Label for="email">Email</Label>
          <Input
            type="email"
            name="email"
            id="email"
            placeholder="user@example.com"
            value={this.state.email}
            invalid={!validEmail(this.state.email)}
            onChange={e => this.setState({ email: e.target.value })}
          />
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
        </FormGroup>
        <FormGroup>
          <Label for="retype-password">Enter Password Again</Label>
          <Input
            type="password"
            name="retype-password"
            id="retype-password"
            placeholder="Password"
            value={this.state.repassword}
            invalid={
              !passwordsEqual(this.state.password, this.state.repassword)
            }
            onChange={e => this.setState({ repassword: e.target.value })}
          />
        </FormGroup>
        <FormGroup>
          <Label for="registrationKey">Registration Key</Label>
          <Input
            type="password"
            name="registrationKey"
            id="registrationKey"
            placeholder="Registration Key"
            value={this.state.registrationKey}
            onChange={e => this.setState({ registrationKey: e.target.value })}
          />
        </FormGroup>
        <span className="form-submit">
          <button onClick={this.handleRegister.bind(this)}>Register</button>
          <SyncLoader
            size={15}
            color={COLORS.SECONDARY.FIRST_DARK}
            loading={this.state.loading}
          />
          <p>{this.state.serviceResponse.username}</p>
        </span>
      </Form>
    );
  }

  async handleRegister(event) {
    event.preventDefault();
    let url = PATHS.REGISTER;
    let data = this.jsonData();
    this.setState({ loading: true });
    try {
      const response = await fetch(url, {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
          "Content-Type": "application/json"
        },
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: data
      });
      let json = await response.json();
      this.updateLoginState(json);
    } catch (ex) {
      this.setState({ serviceResponse: "An error occured" });
    } finally {
      this.setState({ loading: false });
    }
  }

  updateLoginState(serviceResponse){
    this.setState({ serviceResponse });
    const loginState = this.props.loginState;
    loginState.loggedIn= true;
    loginState.showModal = false;
    loginState.username = serviceResponse.username;
    loginState.email = serviceResponse.email;
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
