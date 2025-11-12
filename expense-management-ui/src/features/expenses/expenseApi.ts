import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { Expense, CreateExpenseDto, UpdateExpenseDto } from '../../types/expense';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface PaginatedExpenses {
  items: Expense[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface GetExpensesParams {
  pageNumber?: number;
  pageSize?: number;
  categoryId?: string;
  startDate?: string;
  endDate?: string;
  searchTerm?: string;
}

export const expenseApi = createApi({
  reducerPath: 'expenseApi',
  baseQuery: fetchBaseQuery({
    baseUrl: API_URL,
    prepareHeaders: (headers) => {
      const token = localStorage.getItem('accessToken');
      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ['Expense'],
  endpoints: (builder) => ({
    getExpenses: builder.query<PaginatedExpenses, GetExpensesParams>({
      query: (params) => ({
        url: '/expenses',
        params,
      }),
      providesTags: ['Expense'],
    }),
    createExpense: builder.mutation<string, CreateExpenseDto>({
      query: (expense) => ({
        url: '/expenses',
        method: 'POST',
        body: expense,
      }),
      invalidatesTags: ['Expense'],
    }),
    updateExpense: builder.mutation<void, UpdateExpenseDto>({
      query: (expense) => ({
        url: `/expenses/${expense.id}`,
        method: 'PUT',
        body: expense,
      }),
      invalidatesTags: ['Expense'],
    }),
    deleteExpense: builder.mutation<void, string>({
      query: (id) => ({
        url: `/expenses/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Expense'],
    }),
  }),
});

export const {
  useGetExpensesQuery,
  useCreateExpenseMutation,
  useUpdateExpenseMutation,
  useDeleteExpenseMutation,
} = expenseApi;
