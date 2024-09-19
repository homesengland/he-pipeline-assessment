<template>
    <v-card>
      <v-card-title>Filter Expenses</v-card-title>
  
      <v-row>
        <!-- Category Filter -->
        <v-col cols="12" md="4">
          <v-select
            v-model="filters.category"
            :items="categories"
            label="Category"
          />
        </v-col>
  
        <!-- Start Date Filter -->
        <v-col cols="12" md="4">
          <v-text-field
            v-model="filters.startDate"
            label="Start Date"
            type="date"
            :value="formattedStartDate"
            @change="handleStartDateChange"
          />
        </v-col>
  
        <!-- End Date Filter -->
        <v-col cols="12" md="4">
          <v-text-field
            v-model="filters.endDate"
            label="End Date"
            type="date"
            :value="formattedEndDate"
            @change="handleEndDateChange"
          />
        </v-col>
      </v-row>
    </v-card>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, computed } from 'vue';
  
  export default defineComponent({
    name: 'FilterOptions',
    props: {
      filters: {
        type: Object as () => {
          category: string;
          startDate: Date | null;
          endDate: Date | null;
        },
        required: true,
      },
      onUpdateFilters: {
        type: Function,
        required: true,
      },
    },
    setup(props) {
      const categories = ref(['', 'Food', 'Travel', 'Entertainment', 'Utilities']);
  
      const handleStartDateChange = (e: Event) => {
        const target = e.target as HTMLInputElement;
        const startDate = target.value ? new Date(target.value) : null;
        props.onUpdateFilters({ ...props.filters, startDate });
      };
  
      const handleEndDateChange = (e: Event) => {
        const target = e.target as HTMLInputElement;
        const endDate = target.value ? new Date(target.value) : null;
        props.onUpdateFilters({ ...props.filters, endDate });
      };
  
      const formattedStartDate = computed(() =>
        props.filters.startDate ? props.filters.startDate.toISOString().split('T')[0] : ''
      );
      const formattedEndDate = computed(() =>
        props.filters.endDate ? props.filters.endDate.toISOString().split('T')[0] : ''
      );
  
      return {
        categories,
        handleStartDateChange,
        handleEndDateChange,
        formattedStartDate,
        formattedEndDate,
      };
    },
  });
  </script>
  