<template>
    <v-card>
      <v-form @submit.prevent="handleSubmit">
        <v-card-title>
          Add New Expense
        </v-card-title>
  
        <v-row>
          <!-- Amount Input -->
          <v-col cols="12" md="6">
            <v-text-field
              v-model="amount"
              label="Amount"
              type="number"
              required
            />
          </v-col>
  
          <!-- Category Input -->
          <v-col cols="12" md="6">
            <v-select
              v-model="category"
              :items="categories"
              label="Category"
              required
            />
          </v-col>
  
          <!-- Date Input -->
          <v-col cols="12" md="6">
            <v-text-field
              v-model="date"
              label="Date"
              type="date"
              required
            />
          </v-col>
  
          <!-- Description Input -->
          <v-col cols="12">
            <v-text-field
              v-model="description"
              label="Description"
            />
          </v-col>
        </v-row>
  
        <!-- Submit Button -->
        <v-btn type="submit" color="primary" class="mt-4">
          Add Expense
        </v-btn>
      </v-form>
    </v-card>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from 'vue';
  
  export default defineComponent({
    name: 'ExpenseForm',
    props: {
      onAddExpense: {
        type: Function,
        required: true,
      },
    },
    setup(props) {
      const amount = ref(0);
      const category = ref('');
      const date = ref('');
      const description = ref('');
  
      const categories = ['Food', 'Travel', 'Entertainment', 'Utilities'];
  
      const handleSubmit = () => {
        const newExpense = {
          id: Date.now().toString(),
          amount: amount.value,
          category: category.value,
          date: new Date(date.value),
          description: description.value,
        };
  
        props.onAddExpense(newExpense);
  
        // Reset form
        amount.value = 0;
        category.value = '';
        date.value = '';
        description.value = '';
      };
  
      return { amount, category, date, description, categories, handleSubmit };
    },
  });
  </script>
  