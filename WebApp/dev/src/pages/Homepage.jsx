import { useState, useEffect } from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import { Button, Segment, Table } from 'semantic-ui-react';
import Gallerypage from './GalleryPage';
import ServicesPage from './ServicesPage';
import LoginPage from './LoginPage';
import SignUpPage from './SignupPage';
import UserPage from './UserPage';
import AddPetPage from './AddPetPage';
import AdminDashboard from './AdminDashboard';
import logo from "../assets/logo.png";
import '../css/App.css';

function Homepage() {
  const [userMessage, setUserMessage] = useState('');
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getCurrentUser();
  }, []);

  const getCurrentUser = async () => {
    const token = localStorage.getItem('token');
    if (!token) {
      setUserMessage("You need to log in first.");
      setLoading(false);
      return;
    }

    try {
      const response = await fetch('http://localhost:5094/api/get-current-user', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error('Failed to fetch current user');
      }

      const data = await response.json();
      if (data && data.data) {
        setCurrentUser(data.data);
        setIsLoggedIn(true);
        localStorage.setItem('user', JSON.stringify(data.data));
        console.log("User data:", data.data);
        console.log("User Roles:", data.data.userRoles);
      } else {
        setUserMessage('No user found or not logged in.');
      }
    } catch (error) {
      setUserMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = async () => {
    const confirmed = window.confirm("Are you sure you want to log out?");
    if (!confirmed) {
        return;
    }

    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setIsLoggedIn(false);
    setCurrentUser(null);
    setUserMessage('Logged out successfully.');
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
          <Link to="/gallery">Gallery</Link>

          {isLoggedIn && currentUser && currentUser.userRoles?.includes('Admin') ? (
            <Link to="/admin">Admin Dashboard</Link>
          ) : isLoggedIn && currentUser && currentUser.userRoles?.includes('USER') ? (
            <Link to="/user">User Page</Link>
          ) : null}

        </div>
        <div className="login">
            {isLoggedIn ? (
                <Button onClick={handleLogout} className="logout-button">Log Out</Button>
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
        <Routes>
          <Route path="/" element={<HomeContent />} />
          <Route path="/gallery" element={<Gallerypage />} />
          <Route path="/services" element={<ServicesPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/booking" element={<Booking />} />
          <Route path="/signup" element={<SignUpPage />} />
          <Route path="/user" element={<UserPage />} />
          <Route path="/add-pet" element={<AddPetPage />} />
          <Route path="/admin" element={<AdminDashboard />} />
        </Routes>
      </main>
    </>
  );
}

const HomeContent = () => (
  <div className="home-container">
    <div className="hero-image">
      <h1>Welcome to The Groom Room Pet Spa & Boutique</h1>
      <p>Your pets deserve the best!</p>
      <Link to="/services" className="cta-button">
        Explore Our Services
      </Link>
    </div>
    <div className="about-us-container">
      <div className="about-us">
        <h2>About Us:</h2>
        <p>
          At The Groom Room, we believe every pet deserves to be pampered! Our
          dedicated team of pet lovers is committed to providing exceptional
          grooming services tailored to your furry friend’s individual needs.
        </p>
        <p>
          From baths and haircuts to nail trims and specialty treatments, we use
          high-quality products that are safe for all breeds. Our warm,
          welcoming environment ensures that your pet feels comfortable and
          cared for during their visit.
        </p>
        <p>
          Join our family of happy pets and owners, and discover the difference
          that love and expertise can make in your pet's grooming experience!
        </p>
      </div>
      <div className="logo-container">
        <img src={logo} alt="Logo" />
      </div>
    </div>
    <div className="contact-container">
    <h2>Contact Us</h2>
      <div className="contact-info">
        <p>📞 Phone: 225-588-6045</p>
        <p>📧 Email: thegroomroompetspaboutique@gmail.com</p>
        <p>🏠 Address: 123 Groom St, Pet City, PC 12345</p>
      </div>
    </div>
  </div>
);

const Booking = () => <h1>Booking Page</h1>;

export default Homepage;
