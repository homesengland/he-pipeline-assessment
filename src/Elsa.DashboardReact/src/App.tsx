import React, { useState } from 'react';
import { Expense } from '.interfaces/Expense';
import ExpenseForm from './components/ExpenseForm'
import ExpenseList from './components/ExpenseList'
import FilterOptions from './components/FilterOptions'
import TotalExpenditure from './components/TotalExpenditure'
import { Container, Typography } from '@mui/material';

const App: React.FC = () => {
  // state management
  const [expenses, setExpenses] = useState<Expense[]>([]);
  const [filters, setFilters] = useState({
    category: '',
    startDate: null,
    endDate: null,
  });
  // functions
  const addExpense = (expense: Expense) => {
    setExpenses((prevExpenses) => [...[prevExpenses, expense]]);
  };

  const updateFilters = (newFilters: typeof filters) => {
    setFilters(newFilters);
  };

  const filteredExpenses = expenses.filter((expense) => {
    const matchesCategory =
      filters.category === '' || expense.category === filters.category;
  
    const matchesStartDate =
      !filters.startDate || expense.date >= filters.startDate;
  
    const matchesEndDate =
      !filters.endDate || expense.date <= filters.endDate;
  
    return matchesCategory && matchesStartDate && matchesEndDate;
  });
  

  return (
    <Container>
      {/* Title of the application */}
      <Typography variant="h3" align="center" gutterBottom>
        Expense Tracker
      </Typography>
  
      {/* Display the total expenditure */}
      <TotalExpenditure expenses={filteredExpenses} />
  
      {/* Form to add new expenses */}
      <ExpenseForm onAddExpense={addExpense} />
  
      {/* Filter options to filter the list of expenses */}
      <FilterOptions filters={filters} onUpdateFilters={updateFilters} />
  
      {/* List of filtered expenses */}
      <ExpenseList expenses={filteredExpenses} />
    </Container>
  );
  
};

export default App;