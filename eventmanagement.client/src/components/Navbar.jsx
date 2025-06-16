import React from 'react';
import { NavLink, Link, useMatch } from 'react-router-dom';
const Navbar = () => {
    const isHomeMatch = useMatch("/");
    const isSalesMatch = useMatch("/sales-summary");
    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
            <div className="container-fluid">
                <Link className="navbar-brand" to="/">Events</Link>
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav ms-auto">
                        <li className={isHomeMatch ? "nav-item active" : ""}>
                            <NavLink className="nav-link" to="/">Upcomming Events</NavLink>
                        </li>
                        <li className={isSalesMatch ? "nav-item active" : ""}>
                            <NavLink className="nav-link" to="/sales-summary">Sales summary</NavLink>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;