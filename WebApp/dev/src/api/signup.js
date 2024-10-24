const API_URL = 'http://localhost:5094/api/Users';

export const signUp = async (userData) => {
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
      });
  
      console.log('Response status:', response.status);
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.errors[0] || 'Sign up failed');
      }
  
      const data = await response.json();
      console.log('Sign up successful:', data);
  
      if (data.token) {
        localStorage.setItem('token', data.token);
        console.log('Current JWT Token:', data.token);
      }
  
      return data;
    } catch (error) {
      console.error('Sign up failed:', error.message);
      throw new Error(error.message || 'Sign up failed');
    }
  };