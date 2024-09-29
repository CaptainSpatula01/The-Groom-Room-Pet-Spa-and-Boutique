import React from 'react';
import { Link } from 'react-router-dom';
import '../css/services.css';

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

  return (
    <div className="services-container">
      <h1>Our Services (Placeholders)</h1>
      <div className="service-category">
        <h2>Large Dogs</h2>
        <ul>
          {services.largeDogs.map((service, index) => (
            <li key={index}>{service}</li>
          ))}
        </ul>
      </div>
      <div className="service-category">
        <h2>Medium Dogs</h2>
        <ul>
          {services.mediumDogs.map((service, index) => (
            <li key={index}>{service}</li>
          ))}
        </ul>
      </div>
      <div className="service-category">
        <h2>Small Dogs</h2>
        <ul>
          {services.smallDogs.map((service, index) => (
            <li key={index}>{service}</li>
          ))}
        </ul>
      </div>
      <div className="booking-button-container">
        <Link to="/booking">
          <button className="navigate-button">Book Now</button>
        </Link>
      </div>
    </div>
  );
};

export default ServicesPage;
