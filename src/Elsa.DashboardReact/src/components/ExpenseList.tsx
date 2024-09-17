import React from 'react';
import { Expense } from '../interfaces/Expense';
import ExpenseItem from './ExpenseItem';
import { List, Typography, Paper } from '@mui/material';

interface ExpenseListProps {
  expenses: Expense[];
}

const ExpenseList: React.FC<ExpenseListProps> = ({ expenses }) => {
  if (expenses.length === 0) {
    return (
      <Typography variant="h6" align="center">
        No expenses found.
      </Typography>
    );
  }

  return (
    <Paper style={{ padding: '16px' }}>
      <List>
        {expenses.map((expense) => (
          <ExpenseItem key={expense.id} expense={expense} />
        ))}
      </List>
    </Paper>
  );
};

export default ExpenseList;
