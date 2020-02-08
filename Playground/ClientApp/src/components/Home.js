import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>Hello!</h1>
        <p>Welcome to my single page playground application, built using:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
          <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        </ul>
        <p>This is intended as a testing ground for me to try out new things. All currently implemented sections are available in the navigation.</p>
        <p>The source code is available on my <a href="https://github.com/greensebastian/c-sharp-playground">GitHub</a>.</p>
      </div>
    );
  }
}
