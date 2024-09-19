<template>
    <v-card v-if="expenses.length > 0">
      <v-list>
        <ExpenseItem
          v-for="expense in expenses"
          :key="expense.id"
          :expense="expense"
        />
      </v-list>
    </v-card>
  
    <v-alert v-else type="info" text>
      No expenses found.
    </v-alert>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  import ExpenseItem from './ExpenseItem.vue';
  import { Expense } from '@/interfaces/Expense';
  
  export default defineComponent({
    name: 'ExpenseList',
    components: {
      ExpenseItem,
    },
    props: {
      expenses: {
        type: Array as () => Expense[],
        required: true,
      },
    },
  });
  </script>
  