import React, { Component } from "react";
import { Form, FormGroup, Input, Label } from "reactstrap";
import { connect } from "react-redux";
import actions from "../../../redux/actions";
import "../Login.scss";

class LoginTabComponent extends Component {
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
          />
        </FormGroup>
        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            type="password"
            name="password"
            id="password"
            placeholder="Password"
          />
        </FormGroup>
        <button onClick={this.handleLogin}>Login</button>
      </Form>
    );
  }

  handleLogin(event) {
    event.preventDefault();
    // TODO handle login checking
  }
}

const LoginTab = connect()(LoginTabComponent);
export default LoginTab;
