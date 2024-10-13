import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/SignUp.css'; // Ensure this CSS file contains the necessary styles

const SignUpPage = () => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: ''
  });
  
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Form submitted:', formData);
    // Handle sign-up logic here (e.g., API call)
    navigate('/'); // Redirect after submission
  };

  const handleLoginRedirect = () => {
    navigate('/login'); // Navigate to login page
  };

  const handleCancel = () => {
    navigate('/login'); // Navigate to home or another appropriate page
  };

  return (
    <div className="signup-container">
      <h1>Sign Up</h1>
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit" className="signup-button">Sign Up</button>
        <button type="button" className="cancel-button" onClick={handleCancel}>Cancel</button>
      </form>
      <button className="login-button" onClick={handleLoginRedirect}>Already have an account? Log In</button>
    </div>
  );
};

export default SignUpPage;
