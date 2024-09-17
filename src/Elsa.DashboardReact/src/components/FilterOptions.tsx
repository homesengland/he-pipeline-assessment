import React from 'react';
import {
  TextField,
  MenuItem,
  Paper,
  Typography,
  Box,
} from '@mui/material';

interface FilterOptionsProps {
  filters: {
    category: string;
    startDate: Date | null;
    endDate: Date | null;
  };
  onUpdateFilters: (filters: {
    category: string;
    startDate: Date | null;
    endDate: Date | null;
  }) => void;
}

const FilterOptions: React.FC<FilterOptionsProps> = ({
  filters,
  onUpdateFilters,
}) => {
  const handleCategoryChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onUpdateFilters({ ...filters, category: e.target.value });
  };

  const handleStartDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onUpdateFilters({
      ...filters,
      startDate: e.target.value ? new Date(e.target.value) : null,
    });
  };

  const handleEndDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onUpdateFilters({
      ...filters,
      endDate: e.target.value ? new Date(e.target.value) : null,
    });
  };

  return (
    <Paper style={{ padding: '16px', marginBottom: '16px' }}>
      <Typography variant="h6" gutterBottom>
        Filter Expenses
      </Typography>
      <Box
        component="div"
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', sm: 'repeat(3, 1fr)' },
          gap: 2,
        }}
      >
        {/* Category Filter */}
        <TextField
          label="Category"
          select
          fullWidth
          value={filters.category}
          onChange={handleCategoryChange}
        >
          <MenuItem value="">All</MenuItem>
          <MenuItem value="Food">Food</MenuItem>
          <MenuItem value="Travel">Travel</MenuItem>
          <MenuItem value="Entertainment">Entertainment</MenuItem>
          <MenuItem value="Utilities">Utilities</MenuItem>
        </TextField>

        {/* Start Date Filter */}
        <TextField
          label="Start Date"
          type="date"
          fullWidth
          value={
            filters.startDate
              ? filters.startDate.toISOString().split('T')[0]
              : ''
          }
          onChange={handleStartDateChange}
          InputLabelProps={{
            shrink: true,
          }}
        />

        {/* End Date Filter */}
        <TextField
          label="End Date"
          type="date"
          fullWidth
          value={
            filters.endDate
              ? filters.endDate.toISOString().split('T')[0]
              : ''
          }
          onChange={handleEndDateChange}
          InputLabelProps={{
            shrink: true,
          }}
        />
      </Box>
    </Paper>
  );
};

export default FilterOptions;
