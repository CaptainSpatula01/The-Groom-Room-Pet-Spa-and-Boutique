import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Segment, Table } from 'semantic-ui-react';
import '../css/services.css';

const ServicesPage = () => {
    const [services, setServices] = useState([]);
    const [error, setError] = useState(null); 
    const nav = useNavigate();

    useEffect(() => {
        // Disable scrolling by adding class
        document.body.classList.add('no-scroll');

        const fetchServices = async () => {
            const token = localStorage.getItem('token');
            try {
                const response = await fetch('http://localhost:5094/api/services', {
                  method: 'GET',
                  headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                  },
                });

                if (!response.ok) {
                    setError('Error fetching services');
                }
                const servicesData = await response.json();

                console.log(servicesData);

                setServices(servicesData || []);
            } catch (err) {
                console.error('Error:', err);
                setError('Error fetching services');
            }
        };

        fetchServices();

        return () => {
          document.body.classList.remove('no-scroll');
        };
    }, []);

    if (error) {
        return <div>{error}</div>; // show error
    }
    
    if(services === 0){
        return <p>No services available.. yet.</p>
    }

    return (
        <div className="services-container">
            <div className="services-header">
                <h1 className="page-title">Our Services</h1>
                <div className="services-buttons">
                    <Button
                        type="button"
                        className="login-button" 
                        onClick={() => {
                            // add a modal for service post to api
                        }}
                    >
                        Add Service
                    </Button>
                </div>
            </div>
    
            {/* <div className="service-category">
                <h2>Medium Dogs</h2>
                <ul>
                    {services.mediumDogs.map((service, index) => (
                        <li key={index}>{service}</li>
                    ))}
                </ul>
            </div> */}
    
            {services.length > 0 ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th>Price</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {services.map((service) => (
                            <tr key={service.id}>
                                <td>{service.description}</td>
                                <td>${service.price}</td>
                                <td>
                                    <button
                                        type="button"
                                        className="login-button" 
                                        onClick={() => {
                                            // create button
                                        }}
                                    >
                                        Add
                                    </button>
                                    <button
                                        type="button"
                                        className="login-button"
                                        onClick={() => {
                                            // edit button
                                        }}
                                    >
                                        Edit
                                    </button>
                                    <button
                                        type="button"
                                        className="signup-button"
                                        onClick={() => {
                                            // delete button
                                        }}
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p>No services available</p>
            )}
    
            <div className="home-button">
                <Button
                    className="signup-button"
                    onClick={() => nav('/')}
                >
                    Home
                </Button>
            </div>
        </div>
    );
}
export default ServicesPage;