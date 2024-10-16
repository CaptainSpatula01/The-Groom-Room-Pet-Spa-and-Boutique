import { useState, useEffect } from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import logo from "../assets/logo.png";
import '../App.css';
import ServicesPage from './ServicesPage';
import LoginPage from './LoginPage';
import SignUpPage from './SignupPage';
import UserPage from './UserPage';
import AddPetPage from './AddPetPage';
import { login } from '../api/login'

function Homepage() {
  const [userMessage, setUserMessage] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      setIsLoggedIn(true);
      getCurrentUser();
    }
  }, []);

  const handleLogin = async () => {
    try {
      const user = await login(username, password);
      setUserMessage(`Successfully logged in as: ${user.data.username}`);
      localStorage.setItem('token', user.data.token);
      localStorage.setItem('user', JSON.stringify(user.data.username));
      setIsLoggedIn(true);
      setCurrentUser(user.data);
      getCurrentUser();
    } catch (error) {
      setUserMessage(error.message);
    }
  };

  const handleLogout = async () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setIsLoggedIn(false);
    setCurrentUser(null);
    setUserMessage('Logged out successfully.');
  };

  const getCurrentUser = async () => {
    const token = localStorage.getItem('token');
    console.log("Token: ", token);
    
    if (!token) {
      setUserMessage("You need to log in first.");
      return;
    }

    try {
      const response = await fetch('http://localhost:5094/api/get-current-user', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      console.log("Response status:", response.status);

      if (!response.ok) {
        const errorResponse = await response.json();
        console.error("Error Response: ", errorResponse);
        throw new Error('Failed to fetch current user');
    }

      const data = await response.json();
      console.log("Data received:", data);

      if (data && data.data) {
        setCurrentUser(data.data);
        setUserMessage(`Current logged-in user: ${data.data.userName}`);
      } else {
        setUserMessage('No user found or not logged in.');
      }
    } catch (error) {
      setUserMessage(error.message);
    }
  };

  return (
    <>
      <nav className="navbar">
        <div className="logo">
          <Link to="/">
            <img src={logo} alt="Logo" />
          </Link>
        </div>
        <div className="nav-links">
          <Link to="/">Home</Link>
          <Link to="/services">Services</Link>
          {isLoggedIn && (
            <Link to="user">
                <button className="user-button">User Page</button>
            </Link>
          )}
        </div>
        <div className="login">
            {isLoggedIn ? (
                <button onClick={handleLogout} className="logout-button">Log Out</button>
            ) : (
                <>
                    <Link to="/login">
                        <button className="login-button">Log In</button>
                    </Link>
                    <Link to="/signup">
                        <button className="signup-button">Sign Up</button>
                    </Link>
                </>
            )}
        </div>
      </nav>

      <main>
        <div>
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <button onClick={handleLogin} className="check-user-button">Test Login</button>
        </div>
        {userMessage && <p>{userMessage}</p>}

        <button onClick={getCurrentUser} className="check-user-button">Get Current User</button>
        {currentUser && (
          <div>
            <h3>Current User Details:</h3>
            <p>Username: {currentUser.userName}</p>
            <p>Email: {currentUser.email}</p>
          </div>
        )}

        <Routes>
          <Route path="/" element={<HomeContent />} />
          <Route path="/services" element={<ServicesPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/booking" element={<Booking />} />
          <Route path="/signup" element={<SignUpPage />} />
          <Route path="/user" element={<UserPage />} />
          <Route path="/add-pet" element={<AddPetPage />} />
        </Routes>
        {console.log("Current User in Homepage:", currentUser)}
      </main>
    </>
  );
}

const HomeContent = () => (
  <div className="home-container">
    <div className="header-container">
      <h1>Welcome to The Groom Room Pet Spa & Boutique</h1>
    </div>
    <div className="about-us-container">
      <div className="about-us">
        <h2>About Us:</h2>
        <p>
          We are dedicated to providing the best care for your pets. Our team of professionals
          is passionate about animals and committed to delivering top-notch grooming services.
        </p>
      </div>
      <div className="logo-container">
        <img src={logo} alt="Logo" />
      </div>
    </div>
    <div className="contact-container">
      <div className="contact-info">
        <h2>Contact Us:</h2>
        <p>Phone: (123) 456-7890</p>
        <p>Email: contact@thegroomroom.com</p>
        <p>Address: 123 Groom St, Pet City, PC 12345</p>
      </div>
    </div>
  </div>
);

const Booking = () => <h1>Booking Page</h1>;

export default Homepage;
