const API_URL = 'http://localhost:5094/api/pets';

export const addPet = async (petData, token) => {
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(petData),
      });
  
      if (!response.ok) {
        const errorText = await response.text();
        try {
            const errorJson = JSON.parse(errorText);
            throw new Error(errorJson.message || 'Failed to add pet');
        } catch (parseError) {
            throw new Error(errorText);
        }
      }
  
      return await response.json();
    } catch (error) {
      throw new Error(error.message);
    }
  };
  