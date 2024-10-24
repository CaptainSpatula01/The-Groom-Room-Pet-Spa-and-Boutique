import express from 'express';
import cors from 'cors';

const app = express();

app.use(cors({
    origin: 'http://localhost:5173',
    methods: ['GET', 'POST', 'PUT', 'DELETE'],
    credentials: true
}));

app.use(express.json());

app.post('/api/login', (req, res) => {
    console.log(req.body);
    res.json({ message: 'Login successful' });
});

const PORT = 5094;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
