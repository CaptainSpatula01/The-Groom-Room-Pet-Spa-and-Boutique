import { useState, useEffect } from 'react';
import dog1 from '../gallery pics/dog 1.png';
import dog2 from '../gallery pics/dog 2.png'; 
import dog3 from '../gallery pics/dog 3.png';
import dog4 from '../gallery pics/dog 4.png';
import '../css/gallery.css'; // Import the CSS file

const images = [
    { src: dog1, alt: "Happy Dog 1" },
    { src: dog2, alt: "Happy Dog 2" },
    { src: dog3, alt: "Happy Dog 3" },
    { src: dog4, alt: "Happy Dog 4" }
  ];

const Gallerypage = () => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isPaused, setIsPaused] = useState(false);

  useEffect(() => {
    // Disable scrolling by adding class
    document.body.classList.add('no-scroll');

    const interval = setInterval(() => {
      setCurrentIndex(prevIndex => (prevIndex + 1) % images.length);
    }, 3000);

    return () => clearInterval(interval);
    document.body.classList.remove('no-scroll');
  }, []);

  const nextImage = () => {
    setCurrentIndex((currentIndex + 1) % images.length);
  };

  const prevImage = () => {
    setCurrentIndex((currentIndex - 1 + images.length) % images.length);
  };

  return (
    <body class="no-scroll">
    <div 
      className="container" 
      onMouseEnter={() => setIsPaused(true)} 
      onMouseLeave={() => setIsPaused(false)}
    >
      <h1 className="title">Happy Customers!</h1>
      <p className="description">Check out our happy customers:</p>
      <div className="gallery">
        <img 
          src={images[currentIndex].src} 
          alt={images[currentIndex].alt} 
          className="image" 
        />
        <div className="navigation">
          <button onClick={prevImage} className="button">❮</button>
          <button onClick={nextImage} className="button">❯</button>
        </div>
      </div>
      </div>
    </body>
  );
};

export default Gallerypage;