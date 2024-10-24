import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem('token');
  const user = JSON.parse(localStorage.getItem('user'));
  const isLoggedIn = !!token && !!user;

  if (!isLoggedIn) {
    return <Navigate to="/login" />;
  }

  if (!user.userRoles?.includes('Admin')) {
    return <Navigate to="/login" />;
  }

  return children;
};

export default ProtectedRoute;