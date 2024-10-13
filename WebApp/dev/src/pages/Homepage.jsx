import { Routes, Route, Link } from 'react-router-dom';
import logo from "../assets/logo.png";
import '../css/App.css';
import ServicesPage from './ServicesPage';
import LoginPage from './LoginPage';
import SignUpPage from './SignupPage';
import GalleryPage from './Gallerypage';

function Homepage() {
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
        </div>
        <div className="login">
          <Link to="/login">
            <button className="login-button">Log In</button>
          </Link>
        </div>
      </nav>

      <main>
        <Routes>
          <Route path="/" element={<HomeContent />} />
          <Route path="/services" element={<ServicesPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/booking" element={<Booking />} />
          <Route path="/signup" element={<SignUpPage />} />
          <Route path="/gallery" element={<GalleryPage />} />
        </Routes>
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
