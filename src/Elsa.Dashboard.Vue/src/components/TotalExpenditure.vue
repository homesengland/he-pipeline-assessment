<template>
    <v-card>
      <v-card-title>Total Expenditure: £{{ total }}</v-card-title>
    </v-card>
  </template>
  
  <script lang="ts">
  import { defineComponent, computed } from 'vue';
  import { Expense } from '@/interfaces/Expense';
  
  export default defineComponent({
    name: 'TotalExpenditure',
    props: {
      expenses: {
        type: Array as () => Expense[],
        required: true,
      },
    },
    setup(props) {
      const total = computed(() => {
        return props.expenses.reduce((sum, expense) => sum + expense.amount, 0).toFixed(2);
      });
  
      return { total };
    },
  });
  </script>
  