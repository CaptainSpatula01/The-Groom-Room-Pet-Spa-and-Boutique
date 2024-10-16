import { useNavigate } from 'react-router-dom';
import { signUp } from '../api/signup';
import '../css/SignUp.css';
import { login } from '../api/login';

  const navigate = useNavigate();

    e.preventDefault();
    console.log('Form submitted:', formData);
    // Handle sign-up logic here (e.g., API call)
    navigate('/'); // Redirect after submission
  };

      };

  };

  return (
    <div className="signup-container">
      <h1>Sign Up</h1>
      {userMessage && <p>{userMessage}</p>}
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            required
          />
        </div>
        {error && <p className="error">{error}</p>}
        <button type="submit" className="signup-button">Sign Up</button>
        <button type="button" className="cancel-button" onClick={handleCancel}>Cancel</button>
      </form>
      <button className="login-button" onClick={handleLoginRedirect}>Already have an account? Log In</button>
    </div>
  );
};

