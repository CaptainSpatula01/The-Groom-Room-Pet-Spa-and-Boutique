import React, { useEffect, useState } from 'react';

const AdminDashboard = () => {
  const [totalUsers, setTotalUsers] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
        const token = localStorage.getItem('token');
        try {
            const usersResponse = await fetch('http://localhost:5094/api/users', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
            });
        
            if (!usersResponse.ok) {
                throw new Error('Failed to fetch users.');
            }

            const usersData = await usersResponse.json();
            console.log(usersData);
            setTotalUsers(usersData.data.length);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    fetchData();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
  }

  if (error) {
    return <h2>Error: {error}</h2>; 
  }

  return (
    <div>
      <h1>Admin Dashboard</h1>
      <p>Total Users: {totalUsers}</p>
    </div>
  );
};

export default AdminDashboard;