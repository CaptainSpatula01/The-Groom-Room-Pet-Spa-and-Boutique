import { Routes, Route } from 'react-router-dom';
import AdminDashboard from './pages/AdminDashboard';
import Homepage from './pages/Homepage';
import LoginPage from './pages/LoginPage';
import SignupPage from './pages/SignupPage';
import UserPage from './pages/UserPage';
import AddPetPage from './pages/AddPetPage';
import ServicesPage from './pages/ServicesPage';
import Gallerypage from './pages/GalleryPage';
import ProtectedRoute from './components/ProtectedRoute';

const App = () => {
  return (
    <Routes>
      <Route path="/" element={<Homepage />} />
      <Route path="/gallery" element={<Gallerypage />} />
      <Route path="/services" element={<ServicesPage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignupPage />} />
      <Route path="/user" element={<UserPage />} />
      <Route path="/add-pet" element={<AddPetPage />} />
      <Route
        path="/admin"
        element={
          <ProtectedRoute>
            <AdminDashboard />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default App;