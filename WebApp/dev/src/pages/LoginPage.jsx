import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/Login.css'; 

const LoginPage = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
  
    const handleSubmit = (e) => {
      e.preventDefault();
      // auth
      console.log('Username:', username);
      console.log('Password:', password);
  

      navigate('/');
    };
  
    const handleSignUp = () => {
      navigate('/signup');
    };
  
    return (
      <div className="login-container">
        <h1>Login</h1>
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
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <button type="submit" className="login-button">Log In</button>
        </form>
        <button className="signup-button" onClick={handleSignUp}>Sign Up</button>
      </div>
    );
  };
  
  export default LoginPage;