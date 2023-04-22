import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <header className="flex">
        <Link to="/">Optimized Price</Link>
        <nav className="flex">
          <ul className="navbar-nav flex-grow">
            <Link className="text-dark" to="/">Home</Link>
            <Link className="text-dark" to="/counter">Counter</Link>
            <Link className="text-dark" to="/fetch-data">Fetch data</Link>
          </ul>
        </nav>
      </header>
    );
  }
}
