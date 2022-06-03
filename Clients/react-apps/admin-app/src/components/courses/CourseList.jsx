import { useEffect, useState } from 'react';
import CourseItem from './CourseItem';

const CourseList = () => {
    const [courses, setCourses] = useState([]);
    
    useEffect(() => {
       loadCourses();
    }, []);
    
    const loadCourses = async () => {
        const token = JSON.parse(localStorage.getItem('token'));
        console.log(token);
        
        const url = `${process.env.REACT_APP_BASEURL}/courses/list`;

        const response = await fetch(url, {
            method: `GET`,
            headers: {
                Authorization: `bearer ${token}`
            }
        });
        
        if(!response.ok) {
            console.log('Something went wrong while fetching courses.');
        } else {
            setCourses(await response.json());
        }
    }
    
    const deleteCourse = async (id) => {
        console.log(`Removing course with id ${id}.`);
        const url = `${process.env.REACT_APP_BASEURL}/courses/${id}`;
        
        const response = await fetch(url, {
            method: 'DELETE'
        })
        
        console.log(response);
        
        if(response.status >= 200 && response.status <= 299) {
            console.log('Course removed.');
        } else {
            console.log('Something went wrong while removing course.');
        }
    }
    
    return(
        <table>
            <thead>
                <tr>
                    <th></th>
                    <th>Number</th>
                    <th>Name</th>
                    <th>Length</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                {courses.map((course) => (
                    <CourseItem
                        course={course}
                        key={course.Id}
                        handleDeleteCourse={deleteCourse}
                    />    
                ))}
            </tbody>
        </table>
    );
}

export default CourseList;