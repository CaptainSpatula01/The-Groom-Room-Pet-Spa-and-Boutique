import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { signUp } from '../api/signup';
import '../css/SignUp.css';
import { login } from '../api/login';

const SignupPage = () => {
  const [username, setUsername] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [userMessage, setUserMessage] = useState('');
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    // Disable scrolling by adding class
    document.body.classList.add('no-scroll');

    // Cleanup function to remove class on unmount
    return () => {
        document.body.classList.remove('no-scroll');
    };
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const newUser = {
        username,
        firstName,
        lastName,
        email,
        password,
      };

      const signUpResponse = await signUp(newUser);
      setUserMessage(`Successfully signed up as: ${signUpResponse.data.username}`);
      
      const loginResponse = await login(username, password);
      localStorage.setItem('token', loginResponse.data.token);
      localStorage.setItem('user', JSON.stringify(loginResponse.data.username));
      
      setError(null);
      navigate('/user');
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Sign up failed. Please try again.';
      setUserMessage(errorMessage);
      setError(errorMessage);
    }
  };

  const handleLoginRedirect = () => {
    navigate('/login'); // Navigate to login page
  };

  const handleCancel = () => {
    navigate('/login'); // Navigate to another appropriate page
  };

  return (
    <div className="signup-container">
      <h1 className="signup-title">Create Your Account Here!</h1>
      {userMessage && <p>{userMessage}</p>}
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="firstName">First Name</label>
          <input
            type="text"
            id="firstName"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="lastName">Last Name</label>
          <input
            type="text"
            id="lastName"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        {error && <p className="error">{error}</p>}
        <button type="submit" className="signup-button">Sign Up</button>
        <button type="button" className="cancel-button" onClick={handleCancel}>Cancel</button>
      </form>
      <button className="login-button" onClick={handleLoginRedirect}>Already have an account? Log In</button>
    </div>
  );
};

export default SignupPage;
