import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/navbar/Navbar';
import CourseList from './components/courses/CourseList';

import './utilities.css';
import './styles.css';

const App = () => {
    return (
        <Router>
            <Navbar />
            <main>
                <Routes>
                    <Route path='/' element={<CourseList />} />
                </Routes>
            </main>
        </Router>);
}

export default App;