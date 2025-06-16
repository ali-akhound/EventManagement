import { useEffect, useState, useRef } from 'react';
import { Grid } from 'react-loader-spinner';
import { toast } from 'react-toastify';
import config from '../config';
function Events() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [days, setDays] = useState(30);
    const [sort, setSort] = useState('startsOn');
    const [sortDirection, setSortDirection] = useState('asc');
    const [pageSize, setPageSize] = useState(10);
    const [currentPage, setCurrentPage] = useState(1);
    const fetchedRef = useRef(false);
    const fetchData = async (day) => {
        try {
            setLoading(true);
            const response = await fetch(`${config.apiBaseUrl}/Events/${day}`);
            if (!response.ok) {
                throw new Error('Failed to fetch data');
            }
            const result = await response.json();
            setData(result);
        } catch (error) {
            console.error('Error:', error);
            toast.error('Oops! Something went wrong. Please try again later.');
        } finally {
            setLoading(false);
        }
    };
    const handleDaysChange = async (day) => {
        setDays(day);
        setCurrentPage(1);
        await fetchData(day);
    }
    const handleSortChange = (col) => {
        if (col === sort) {
            setSortDirection(prevDirection => prevDirection === 'asc' ? 'desc' : 'asc');
        }
        else {
            setSortDirection('asc');
        }
        setSort(col);
    }
    const handleNextPage = () => {
        setCurrentPage(prevPage => prevPage < Math.ceil(data.length / pageSize) ? prevPage + 1 : prevPage);
    }
    const handlePrevPage = () => {
        setCurrentPage(prevPage => prevPage > 1 ? prevPage - 1 : 1);
    }
    useEffect(() => {
        if (fetchedRef.current) return; // skip if already fetched
        fetchedRef.current = true;
        fetchData(days);
    }, []);
    return (
        <>
            {loading ?
                <div className="spinner-container">
                    <Grid
                        visible={loading}
                        height="80"
                        width="80"
                        color="#d9172c"
                        ariaLabel="grid-loading"
                        radius="12.5"
                        wrapperStyle={{}}
                        wrapperClass="grid-wrapper"
                    />
                </div>
                :
                <>
                    <div className="dropdown">
                        <button className="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Upcoming Events in {days} days
                        </button>
                        <ul className="dropdown-menu">
                            <li><a className="dropdown-item" onClick={async () => await handleDaysChange(30)} href="#">30 days</a></li>
                            <li><a className="dropdown-item" onClick={async () => await handleDaysChange(60)} href="#">60 days</a></li>
                            <li><a className="dropdown-item" onClick={async () => await handleDaysChange(180)} href="#">180 days</a></li>
                        </ul>
                    </div>
                    <table className="table">
                        <thead>
                            <tr>
                                <th scope="col">#</th>
                                <th scope="col">
                                    <a
                                        href="#"
                                        onClick={() => handleSortChange('name')}
                                        className={sort === 'name' ? 'active' : ''}>
                                        Name
                                        {sort === 'name' && (
                                            sortDirection === 'asc' ? ' ▲' : ' ▼'
                                        )}
                                    </a>
                                </th>
                                <th scope="col">
                                    <a
                                        href="#"
                                        onClick={() => handleSortChange('startsOn')}
                                        className={sort === 'startsOn' ? 'active' : ''}
                                    >
                                        Starts On
                                        {sort === 'startsOn' && (
                                            sortDirection === 'asc' ? ' ▲' : ' ▼'
                                        )}
                                    </a>
                                </th>
                                <th scope="col">
                                    <a
                                        href="#"
                                        onClick={() => handleSortChange('endsOn')}
                                        className={sort === 'endsOn' ? 'active' : ''}
                                    >
                                        Ends On
                                        {sort === 'endsOn' && (
                                            sortDirection === 'asc' ? ' ▲' : ' ▼'
                                        )}
                                    </a>
                                </th>
                                <th scope="col">
                                    <a
                                        href="#"
                                        onClick={() => handleSortChange('location')}
                                        className={sort === 'location' ? 'active' : ''}
                                    >
                                        Location
                                        {sort === 'location' && (
                                            sortDirection === 'asc' ? ' ▲' : ' ▼'
                                        )}
                                    </a>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                data
                                    .slice((currentPage - 1) * pageSize, currentPage * pageSize)
                                    .sort((a, b) => {
                                        let result = 0;
                                        switch (sort) {
                                            case 'startsOn':
                                                result = new Date(a.startsOn) - new Date(b.startsOn);
                                                break;
                                            case 'endsOn':
                                                result = new Date(a.endsOn) - new Date(b.endsOn);
                                                break;
                                            case 'location':
                                                result = a.location.localeCompare(b.location);
                                                break;
                                            case 'name':
                                                result = a.name.localeCompare(b.name);
                                                break;
                                            default:
                                                result = new Date(a.startsOn) - new Date(b.startsOn);
                                        }
                                        return sortDirection === 'asc' ? result : -result;
                                    })
                                    .map((event, index) => (
                                        <tr key={event.id}>
                                            <th scope="row">{((currentPage - 1) * pageSize) + (index + 1)}</th>
                                            <td>{event.name}</td>
                                            <td>{new Date(event.startsOn).toISOString().slice(0, 16).replace('T', ' ') + ':00'}</td>
                                            <td>{new Date(event.endsOn).toISOString().slice(0, 16).replace('T', ' ') + ':00'}</td>
                                            <td>{event.location}</td>
                                        </tr>
                                    ))
                            }
                        </tbody>
                    </table>
                    <nav aria-label="Page navigation example">
                        <ul className="pagination">
                            <li className="page-item">
                                <button className="page-link" onClick={handlePrevPage}>
                                    <span aria-hidden="true">&laquo;</span>
                                </button>
                            </li>
                            <li className="page-item"><span className="page-link" href="#">{currentPage} of {Math.ceil(data.length / pageSize)}</span></li>
                            <li className="page-item">
                                <button className="page-link" onClick={handleNextPage}>
                                    <span aria-hidden="true">&raquo;</span>
                                </button>
                            </li>
                        </ul>
                    </nav>
                </>
            }
        </>
    );

}

export default Events;