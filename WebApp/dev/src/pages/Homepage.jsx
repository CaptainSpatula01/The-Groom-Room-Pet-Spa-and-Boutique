import { Routes, Route, Link } from "react-router-dom";
import logo from "../assets/logo.png";
import "../css/App.css";
import ServicesPage from "./ServicesPage";
import LoginPage from "./LoginPage";
import SignUpPage from "./SignupPage";
import GalleryPage from "./Gallerypage";

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
    <div className="hero-image">
      <h1>Welcome to The Groom Room Pet Spa & Boutique!</h1>
      <p>Your pets deserve the best!</p>
      <Link to="/services" className="cta-button">
        Explore Our Services
      </Link>
    </div>
    <div className="about-us-container">
      <div className="about-us">
        <h2>About Us</h2>
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
