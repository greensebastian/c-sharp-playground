import React, { Component } from "react";
import { Form, FormGroup, Input, Label } from "reactstrap";
import { connect } from "react-redux";
import actions from "../../../redux/actions";
import "../Login.scss";

class RegisterTabComponent extends Component {
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
        <FormGroup>
          <Label for="retype-password">Enter Password Again</Label>
          <Input
            type="password"
            name="retype-password"
            id="retype-password"
            placeholder="Password"
          />
        </FormGroup>
        <FormGroup>
          <Label for="registrationKey">Registration Key</Label>
          <Input
            type="password"
            name="registrationKey"
            id="registrationKey"
            placeholder="Registration Key"
          />
        </FormGroup>
        <button onClick={this.handleRegister}>Register</button>
      </Form>
    );
  }

  handleRegister(event) {
    event.preventDefault();
    // TODO handle login checking
  }
}

const RegisterTab = connect()(RegisterTabComponent);
export default RegisterTab;
