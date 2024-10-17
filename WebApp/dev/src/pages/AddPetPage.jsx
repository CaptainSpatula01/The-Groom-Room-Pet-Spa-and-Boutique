import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { addPet } from '../api/addpet';

const AddPetPage = () => {
  const [petName, setPetName] = useState('');
  const [petBreed, setPetBreed] = useState('');
  const [petSize, setPetSize] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    const token = localStorage.getItem('token');

    if (!token) {
      setMessage("You need to log in first.");
      return;
    }

    const petData = {
      name: petName,
      breed: petBreed,
      size: petSize,
    };

    try {
        await addPet(petData, token);
        setMessage('Pet added successfully!');
        setPetName('');
        setPetBreed('');
        setPetSize('');
        navigate('/user');
    } catch (error) {
        setMessage(error.message);
    }
  };

  return (
    <div>
      <h2>Add a Pet</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">Name:</label>
          <input
            type="text"
            id="name"
            value={petName}
            onChange={(e) => setPetName(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="breed">Breed:</label>
          <input
            type="text"
            id="breed"
            value={petBreed}
            onChange={(e) => setPetBreed(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="size">Size:</label>
          <input
            type="text"
            id="size"
            value={petSize}
            onChange={(e) => setPetSize(e.target.value)}
            required
          />
        </div>
        <button type="submit">Add Dog</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
};

export default AddPetPage;
