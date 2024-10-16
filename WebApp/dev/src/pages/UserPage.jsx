import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

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
    } catch (error) {
      setUserMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <h2>Loading user information...</h2>;
  }

  if (!currentUser) {
    return <h2>No user information available. Please log in.</h2>;
  }

  return (
    <div>
        <h2>User Details</h2>
        <p>Username: {currentUser.userName}</p>
        <p>First Name: {currentUser.firstName}</p>
        <p>Last Name: {currentUser.lastName}</p>
        <p>Email: {currentUser.email}</p>
        <Link to="/add-pet">
            <button>Add a Pet</button>
        </Link>
    </div>
  );
};

export default UserPage;
