import React from 'react';
import { Expense } from '../interfaces/Expense';
import { Typography, Paper } from '@mui/material';

interface TotalExpenditureProps {
  expenses: Expense[];
}

const TotalExpenditure: React.FC<TotalExpenditureProps> = ({ expenses }) => {
  const total = expenses.reduce((sum, expense) => sum + expense.amount, 0);

  return (
    <Paper style={{ padding: '16px', marginBottom: '16px' }}>
      <Typography variant="h5" align="center">
        Total Expenditure: £{total.toFixed(2)}
      </Typography>
    </Paper>
  );
};

export default TotalExpenditure;
