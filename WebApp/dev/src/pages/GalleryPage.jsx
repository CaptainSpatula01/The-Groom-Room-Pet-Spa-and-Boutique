import React, { useState, useEffect } from 'react';
import dog1 from '../gallery pics/dog 1.png';
import dog2 from '../gallery pics/dog 2.png'; 
import dog3 from '../gallery pics/dog 3.png';
import dog4 from '../gallery pics/dog 4.png';

const images = [dog1, dog2, dog3, dog4];

const Gallerypage = () => {
  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentIndex(prevIndex => (prevIndex + 1) % images.length);
    }, 3000); // Change image every 3 seconds

    return () => clearInterval(interval); // Cleanup on unmount
  }, []);

  const nextImage = () => {
    setCurrentIndex((currentIndex + 1) % images.length);
  };

  const prevImage = () => {
    setCurrentIndex((currentIndex - 1 + images.length) % images.length);
  };

  return (
    <div>
      <h1>Our Happy Customers!</h1>
      <p>Here are some pictures of our happy customers:</p>
      <div className="gallery">
        <img 
          src={images[currentIndex]} 
          alt={`Happy Dog ${currentIndex + 1}`} 
          style={{ width: '500px', height: 'auto', margin: '10px', display: 'block', marginLeft: 'auto', marginRight: 'auto' }} 
        />
      </div>
      <div style={{ textAlign: 'center', marginTop: '20px' }}>
        <button onClick={prevImage} style={{ marginRight: '10px' }}>Previous</button>
        <button onClick={nextImage}>Next</button>
      </div>
    </div>
  );
};

export default Gallerypage;
