import React, { useState } from 'react';

const AddPetPage = () => {
  const [petName, setPetName] = useState('');
  const [petBreed, setPetBreed] = useState('');
  const [petSize, setPetSize] = useState('');
  const [message, setMessage] = useState('');

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
      const response = await fetch('http://localhost:5094/api/pets', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(petData),
      });

      if (!response.ok) {
        const errorResponse = await response.json();
        throw new Error(errorResponse.message || 'Failed to add pet');
      }

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
          <label>Name:</label>
          <input
            type="text"
            value={petName}
            onChange={(e) => setPetName(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Breed:</label>
          <input
            type="text"
            value={petBreed}
            onChange={(e) => setPetBreed(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Size:</label>
          <input
            type="text"
            value={petSize}
            onChange={(e) => setPetSize(e.target.value)}
            required
          />
        </div>
        <button type="submit">Add Pet</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
};

export default AddPetPage;
