import { useNavigate } from 'react-router-dom';

const CourseItem = ({course, handleDeleteCourse}) => {
    const navigate = useNavigate();
    
    const onEditClickHandler = () => {
      navigate(`/edit/${course.courseNo}`);  
    };
    
    const onDeleteClickHandler = () => {
      handleDeleteCourse(course.courseNo);  
    };
    
    return (
        <tr>
            <td>
                <span onClick={onEditClickHandler}>
                    <i>
                        <i className='fa-solid fa-pencil edit'></i>
                    </i>
                </span>
            </td>
            <td>{course.courseNo}</td>
            <td>{course.title}</td>
            <td>{course.length}</td>
            <td>
                <span>
                    <i className='fa-solid fa-trash-can delete'></i>
                </span>
            </td>
        </tr>
    );
}

export default CourseItem;