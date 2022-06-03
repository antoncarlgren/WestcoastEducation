import { NavLink } from 'react-router-dom';

const Navbar = ()  => {
    return(
        <nav id='navbar'>
            <h1 className='logo'>
                <span className='text-primary'>
                    <i className='fas fa-user-graduate'></i>
                    Westcoast Education
                </span>
            </h1>
            <ul>
                <li>
                    <NavLink to='/'>Home</NavLink>
                    <NavLink to='/courses/list'>Courses</NavLink>
                    <NavLink to='/teachers/list'>Teachers</NavLink>
                    <NavLink to='/students/list'>Students</NavLink>
                    <NavLink to='/categories/list'>Categories</NavLink>
                </li>
            </ul>
        </nav>
    );
}

export default Navbar;