import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Button } from 'semantic-ui-react';
import '../css/User.css';

const UserPage = () => {
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [userMessage, setUserMessage] = useState('');

  useEffect(() => {
    getCurrentUser();
  }, []);

  const getCurrentUser = async () => {
    const token = localStorage.getItem('token');
    if (!token) {
      setUserMessage("You need to log in first.");
      setLoading(false);
      return;
    }

    try {
      const response = await fetch('http://localhost:5094/api/get-current-user', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        const errorResponse = await response.json();
        throw new Error('Failed to fetch current user');
      }

      const data = await response.json();
      if (data && data.data) {
        setCurrentUser(data.data);
        setUserMessage(`Current logged-in user: ${data.data.userName}`);
      } else {
        setUserMessage('No user found or not logged in.');
      }
      console.log(data.data);
    } catch (error) {
      setUserMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  const handleRemovePet = async (petId) => {
    const confirmed = window.confirm("Are you sure you want to remove this pet from your account?");
    if (!confirmed) {
        return;
    }

    const token = localStorage.getItem('token');
    try {
        const response = await fetch(`http://localhost:5094/api/pets/${petId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
        });

        if (response.ok) {
            setCurrentUser((prevUser) => ({
                ...prevUser,
                pets: prevUser.pets.filter((pet) => pet.id !== petId),
            }));
            setUserMessage('Pet removed successfully.');
        } else {
            throw new Error('Failed to remove pet.');
        }
    } catch (error) {
        setUserMessage(error.message);
    }
  };

  if (loading) {
    return <h2>Loading user information...</h2>;
  }

  if (!currentUser) {
    return <h2>No user information available. Please log in.</h2>;
  }

  return (
    <div className="user-page-container">
      <Link to="/">
        <Button className="homepage-button">Return to Homepage</Button>
      </Link>
      
      <h2>User Details</h2>
      
      <div className="user-info">
        <p>Username: {currentUser.userName}</p>
        <p>First Name: {currentUser.firstName}</p>
        <p>Last Name: {currentUser.lastName}</p>
        <p>Email: {currentUser.email}</p>
      </div>

      {currentUser.pets && currentUser.pets.length > 0 ? (
        <div className="pets-container">
          <h3>Your Dogs:</h3>
          {currentUser.pets.map((pet) => (
            <div key={pet.id} className="pet-item">
              <p>Name: {pet.name}</p>
              <p>Breed: {pet.breed}</p>
              <p>Size: {pet.size}</p>
              <Button className="remove-button" onClick={() => handleRemovePet(pet.id)}>
                Remove Pet
              </Button>
            </div>
          ))}
        </div>
      ) : (
        <p>No dogs added yet.</p>
      )}

      <Link to="/add-pet">
        <Button className="add-button">Add a Pet</Button>
      </Link>
    </div>
  );
};

export default UserPage;
