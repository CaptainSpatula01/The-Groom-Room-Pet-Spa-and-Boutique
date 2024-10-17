import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/SignUp.css';

const SignUpPage = () => {
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        password: ''
    });

    const navigate = useNavigate();

    useEffect(() => {
        // Disable scrolling by adding class
        document.body.classList.add('no-scroll');

        // Cleanup function to remove class on unmount
        return () => {
            document.body.classList.remove('no-scroll');
        };
    }, []);

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
        navigate('/login'); // Navigate to another appropriate page
    };

    return (
        <div className="signup-container">
            <h1 className="signup-title">Create Your Account Here!</h1>
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
                        className="form-input"
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
                        className="form-input"
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
                        className="form-input"
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
