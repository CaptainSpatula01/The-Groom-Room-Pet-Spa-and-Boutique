import React, { useEffect, useState } from 'react';

const AdminDashboard = () => {
  const [totalUsers, setTotalUsers] = useState(0);
  const [totalAppointments, setTotalAppointments] = useState(0);

  useEffect(() => {
    // Fetch the total users and appointments from your API
    const fetchData = async () => {
      const usersResponse = await fetch('/api/users'); // Adjust the endpoint as needed
      const usersData = await usersResponse.json();
      setTotalUsers(usersData.length); // Or however you count users

      const appointmentsResponse = await fetch('/api/appointments'); // Adjust the endpoint as needed
      const appointmentsData = await appointmentsResponse.json();
      setTotalAppointments(appointmentsData.length); // Or however you count appointments
    };

    fetchData();
  }, []);

  return (
    <div>
      <h1>Admin Dashboard</h1>
      <p>Total Users: {totalUsers}</p>
      <p>Total Appointments: {totalAppointments}</p>
    </div>
  );
};

export default AdminDashboard;