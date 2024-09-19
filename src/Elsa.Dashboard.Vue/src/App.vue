<template>
  <v-container>
    <!-- Title of the application -->
    <v-row justify="center">
      <v-col cols="12">
        <v-typography variant="h3" align="center" class="mb-4">
          Expense Tracker
        </v-typography>
      </v-col>
    </v-row>

    <!-- Display the total expenditure -->
    <v-row justify="center">
      <v-col cols="12" md="6">
        <TotalExpenditure :expenses="filteredExpenses" />
      </v-col>
    </v-row>

    <!-- Form to add new expenses -->
    <v-row justify="center">
      <v-col cols="12" md="6">
        <ExpenseForm @onAddExpense="addExpense" />
      </v-col>
    </v-row>

    <!-- Filter options to filter the list of expenses -->
    <v-row justify="center">
      <v-col cols="12" md="6">
        <FilterOptions :filters="filters" @onUpdateFilters="updateFilters" />
      </v-col>
    </v-row>

    <!-- List of filtered expenses -->
    <v-row justify="center">
      <v-col cols="12" md="6">
        <ExpenseList :expenses="filteredExpenses" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from 'vue';
import ExpenseForm from './components/ExpenseForm.vue';
import ExpenseList from './components/ExpenseList.vue';
import FilterOptions from './components/FilterOptions.vue';
import TotalExpenditure from './components/TotalExpenditure.vue';
import { Expense } from '@/interfaces/Expense';

export default defineComponent({
  name: 'App',
  components: {
    ExpenseForm,
    ExpenseList,
    FilterOptions,
    TotalExpenditure,
  },
  setup() {
    const expenses = ref<Expense[]>([]);
    const filters = ref({
      category: '',
      startDate: null as Date | null,
      endDate: null as Date | null,
    });

    const addExpense = (expense: Expense) => {
      expenses.value.push(expense);
    };

    const updateFilters = (newFilters: typeof filters.value) => {
      filters.value = { ...newFilters };
    };

    const filteredExpenses = computed(() => {
      return expenses.value.filter((expense) => {
        const matchesCategory =
          !filters.value.category || expense.category === filters.value.category;

        const matchesStartDate =
          !filters.value.startDate || expense.date >= filters.value.startDate;

        const matchesEndDate =
          !filters.value.endDate || expense.date <= filters.value.endDate;

        return matchesCategory && matchesStartDate && matchesEndDate;
      });
    });

    return {
      expenses,
      filters,
      addExpense,
      updateFilters,
      filteredExpenses,
    };
  },
});
</script>
