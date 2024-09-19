<template>
    <v-list-item>
      <v-list-item-avatar>
        <v-avatar>
          <v-icon>mdi-currency-gbp</v-icon>
        </v-avatar>
      </v-list-item-avatar>
      <v-list-item-content>
        <v-list-item-title>{{ expense.category }}: £{{ formattedAmount }}</v-list-item-title>
        <v-list-item-subtitle>Date: {{ formattedDate }}{{ descriptionText }}</v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  import { Expense } from '@/interfaces/Expense';
  
  export default defineComponent({
    name: 'ExpenseItem',
    props: {
      expense: {
        type: Object as () => Expense,
        required: true,
      },
    },
    computed: {
      formattedAmount(): string {
        return this.expense.amount.toFixed(2);
      },
      formattedDate(): string {
        return new Date(this.expense.date).toLocaleDateString();
      },
      descriptionText(): string {
        return this.expense.description ? ` - ${this.expense.description}` : '';
      },
    },
  });
  </script>
  