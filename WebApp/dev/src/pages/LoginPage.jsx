import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/Login.css';

const LoginPage = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        // Disable scrolling by adding class
        document.body.classList.add('no-scroll');

        // Cleanup function to remove class on unmount
        return () => {
            document.body.classList.remove('no-scroll');
        };
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        // Add your authentication logic here
        console.log('Username:', username);
        console.log('Password:', password);
        navigate('/');
    };

    const handleSignUp = () => {
        navigate('/signup');
    };

    return (
        <div className="login-container">
            <h1 className="login-title">Ready for that Spa Day?</h1>
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
                <button type="submit" className="login-button">Log In</button>
            </form>
            <button className="signup-button" onClick={handleSignUp}>Create Account</button>
        </div>
    );
};

export default LoginPage;
