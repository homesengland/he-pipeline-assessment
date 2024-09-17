import React, { useState } from 'react';
import { Expense } from '../interfaces/Expense';
import {
  TextField,
  Button,
  MenuItem,
  Box,
  Paper,
  Typography
} from '@mui/material';

interface ExpenseFormProps {
    onAddExpense: (expense: Expense) => void; 
}

const ExpenseForm: React.FC<ExpenseFormProps> = ({ onAddExpense }) => {
    const [amount, setAmount] = useState<number>(0);
    const [category, setCategory] = useState<string>('');
    const [date, setDate] = useState<string>('');
    const [description, setDescription] = useState<string>('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
    
        const newExpense: Expense = {
          id: Date.now().toString(),
          amount,
          category,
          date: new Date(date),
          description,
        };
    
        onAddExpense(newExpense);
    
        setAmount(0);
        setCategory('');
        setDate('');
        setDescription('');
    };

    return (
        <Paper style={{ padding: '16px', marginBottom: '16px' }}>
          <form onSubmit={handleSubmit}>
            <Typography variant="h6" gutterBottom>
              Add New Expense
            </Typography>
            <Box
              component="div"
              sx={{
                display: 'grid',
                gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' },
                gap: 2,
              }}
            >
              {/* Amount Input */}
              <TextField
                label="Amount"
                type="number"
                fullWidth
                value={amount}
                onChange={(e) => setAmount(parseFloat(e.target.value))}
                required
              />
              {/* Category Input */}
              <TextField
                label="Category"
                select
                fullWidth
                value={category}
                onChange={(e) => setCategory(e.target.value)}
                required
              >
                <MenuItem value="Food">Food</MenuItem>
                <MenuItem value="Travel">Travel</MenuItem>
                <MenuItem value="Entertainment">Entertainment</MenuItem>
                <MenuItem value="Utilities">Utilities</MenuItem>
              </TextField>
              {/* Date Input */}
              <TextField
                label="Date"
                type="date"
                fullWidth
                value={date}
                onChange={(e) => setDate(e.target.value)}
                required
                InputLabelProps={{
                  shrink: true,
                }}
              />
              {/* Description Input */}
              <TextField
                label="Description"
                fullWidth
                value={description}
                onChange={(e) => setDescription(e.target.value)}
              />
            </Box>
            {/* Submit Button */}
            <Box mt={2}>
              <Button type="submit" variant="contained" color="primary" fullWidth>
                Add Expense
              </Button>
            </Box>
          </form>
        </Paper>
    );
}; 

export default ExpenseForm;
