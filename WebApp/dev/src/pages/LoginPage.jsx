import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../api/login';
import '../css/Login.css';

const LoginPage = () => {
  const [userMessage, setUserMessage] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const [redirectMessage, setRedirectMessage] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    document.body.classList.add('no-scroll');
    
    const token = localStorage.getItem('token');
    if (token) {
      setRedirectMessage('You are already logged in. Redirecting to the home page...');
      setTimeout(() => {
        navigate('/');
      }, 3000);
    }

    return () => {
      document.body.classList.remove('no-scroll');
    };
  }, [navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const user = await login(username, password);
      setUserMessage(`Successfully logged in as: ${user.data.username}`);
      localStorage.setItem('token', user.data.token);
      localStorage.setItem('user', JSON.stringify(user.data.username));
      setError(null);
      navigate('/user');
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Login failed, please check your credentials.';
      setUserMessage(errorMessage);
      setError(errorMessage);
    }
  };

  const handleSignUp = () => {
    navigate('/signup');
  };

  return (
    <div className="login-container">
    <div className="login-box"> {/* Combined container for title and form */}
      <h1 className="login-title">Ready for that Spa Day?</h1>
      {redirectMessage && <p className="redirect-message">{redirectMessage}</p>}
      
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            className="form-input"
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
            className="form-input"
          />
        </div>
        
        {error && <p className="error">{error}</p>}
        {userMessage && <p>{userMessage}</p>}
        
        <div className="buttons-container">
          <button type="submit" className="login-button">Log In</button>
          <button type="button" className="signup-button" onClick={handleSignUp}>Create Account</button>
        </div>
      </form>
    </div>
  </div>
  );
};

export default LoginPage;
