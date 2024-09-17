import React from 'react';
import { Expense } from '../interfaces/Expense';
import {
  ListItem,
  ListItemText,
  ListItemAvatar,
  Avatar,
} from '@mui/material';
import { Money } from '@mui/icons-material';

interface ExpenseItemProps {
  expense: Expense;
}

const ExpenseItem: React.FC<ExpenseItemProps> = ({ expense }) => {
  return (
    <ListItem>
      <ListItemAvatar>
        <Avatar>
          <Money />
        </Avatar>
      </ListItemAvatar>
      <ListItemText
        primary={`${expense.category}: $${expense.amount.toFixed(2)}`}
        secondary={`Date: ${expense.date.toLocaleDateString()}${
          expense.description ? ` - ${expense.description}` : ''
        }`}
      />
    </ListItem>
  );
};

export default ExpenseItem;
