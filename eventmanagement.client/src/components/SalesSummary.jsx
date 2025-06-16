import { useEffect, useState, useRef } from 'react';
import { Grid } from 'react-loader-spinner';
import { toast } from 'react-toastify';
import config from '../config';
function SalesSummary() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [sort, setSort] = useState('startsOn');
    const [sortDirection, setSortDirection] = useState('asc');
    const fetchedRef = useRef(false);
    const fetchData = async () => {
        try {
            setLoading(true);
            const response = await fetch(`${config.apiBaseUrl}/Events/GetTop5EventsBySalesCount`);
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

    const handleSortChange = (col) => {
        if (col === sort) {
            setSortDirection(prevDirection => prevDirection === 'asc' ? 'desc' : 'asc');
        }
        else {
            setSortDirection('asc');
        }
        setSort(col);
    }
    useEffect(() => {
        if (fetchedRef.current) return; // skip if already fetched
        fetchedRef.current = true;
        fetchData();
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
                                .slice()
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
                                        <th scope="row">{index + 1}</th>
                                        <td>{event.name}</td>
                                        <td>{new Date(event.startsOn).toISOString().slice(0, 16).replace('T', ' ') + ':00'}</td>
                                        <td>{new Date(event.endsOn).toISOString().slice(0, 16).replace('T', ' ') + ':00'}</td>
                                        <td>{event.location}</td>
                                    </tr>
                                ))
                        }
                    </tbody>
                </table>
            }
        </>
    );

}

export default SalesSummary;