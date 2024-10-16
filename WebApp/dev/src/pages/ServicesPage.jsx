import { useEffect, useState } from 'react';
import axios from 'axios';
import { Button, Segment, Table } from 'semantic-ui-react';
import { useNavigate } from 'react-router-dom';
import '../css/services.css';

const ServicesPage = () => {
    const [services, setServices] = useState([]);
    const [error, setError] = useState(null); 
    const nav = useNavigate();

    /* THESE WERE THE PLACEHOLDER SERVICES THAT NEED TO BE LOADED INTO THE DATABASE (CHECK HER WEBSITE FOR THE ACTUAL SERVICES FOR SEEDING)
    const ServicesPage = () => {
  const services = {
    largeDogs: [
      'Nail Clipping',
      'Bathing',
      'Haircut',
      'De-Shedding Treatment',
      'Teeth Cleaning',
    ],
    mediumDogs: [
      'Nail Clipping',
      'Bathing',
      'Haircut',
      'Ear Cleaning',
      'Flea Treatment',
    ],
    smallDogs: [
      'Nail Clipping',
      'Bathing',
      'Haircut',
      'Teeth Cleaning',
      'Grooming',
    ],
  };
  */

    useEffect(() => {
        const fetchServices = async () => {
            try {
                const response = await axios.get('api/services');
                const { data, hasErrors } = response.data;

                if (hasErrors) {
                    setError('Error fetching services');
                } else {
                    setServices(data || []); // grabs from database
                }
            } catch (err) {
                console.error('Error:', err);
                setError('Error fetching services');
            }
        };

        fetchServices();
    }, []);

    if (error) {
        return <div>{error}</div>; // show error
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
      <div className="service-category">
        <h2>Medium Dogs</h2>
        <ul>
          {services.mediumDogs.map((service, index) => (
            <li key={index}>{service}</li>
          ))}
        </ul>
      </div>

            {services.length > 0 ? (
                <Segment inverted>
                    <Table celled inverted>
                        <Table.Header>
                            <Table.Row>
                                <Table.HeaderCell>Description</Table.HeaderCell>
                                <Table.HeaderCell>Price</Table.HeaderCell>
                                <Table.HeaderCell>Actions</Table.HeaderCell>
                            </Table.Row>
                        </Table.Header>
                        <Table.Body>
                            {services.map((service) => (
                                <Table.Row key={service.id}>
                                    <Table.Cell>{service.description}</Table.Cell>
                                    <Table.Cell>${service.price}</Table.Cell>
                                    <Table.Cell>
                                        <Button
                                            type="button"
                                            className="login-button" 
                                            onClick={() => {
                                                // create button
                                            }}
                                        >
                                            Add
                                        </Button>
                                        <Button
                                            type="button"
                                            className="login-button"
                                            onClick={() => {
                                                // edit button
                                            }}
                                        >
                                            Edit
                                        </Button>
                                        <Button
                                            type="button"
                                            className="signup-button"
                                            onClick={() => {
                                                // delete button
                                            }}
                                        >
                                            Delete
                                        </Button>
                                    </Table.Cell>
                                </Table.Row>
          ))}
                        </Table.Body>
                    </Table>
                </Segment>
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
};

export default ServicesPage;