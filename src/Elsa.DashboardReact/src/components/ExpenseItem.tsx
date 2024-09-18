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
  const amount = expense.amount ? expense.amount.toFixed(2) : '0.00';
  const formattedDate = expense.date ? expense.date.toLocaleDateString() : 'Invalid date';

  return (
    <ListItem>
      <ListItemAvatar>
        <Avatar>
          <Money />
        </Avatar>
      </ListItemAvatar>
      <ListItemText
        primary={`${expense.category}: £${amount}`}
        secondary={`Date: ${formattedDate}${
          expense.description ? ` - ${expense.description}` : ''
        }`}
      />
    </ListItem>
  );
};

export default ExpenseItem;