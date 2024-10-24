const API_URL = 'http://localhost:5094/api/authenticate';

export const login = async (username, password) => {
  try {
    const response = await fetch(`${API_URL}`, { 
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ UserName: username, Password: password }),
    });

    console.log('Response status:', response.status);

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.errors[0] || 'Login failed');
    }

    const data = await response.json();

    if (data.token) {
      localStorage.setItem('token', data.token);
      const token = localStorage.getItem('token');
      console.log('Current JWT Token:', token);
    }

    console.log('Login successful:', data);

    return data;
  } catch (error) {
    console.error('Login failed:', error.message);
    throw new Error(error.message || 'Login failed');
  }
};
