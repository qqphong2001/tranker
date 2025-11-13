import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface Budget {
  id: string;
  name: string;
  amount: number;
  spentAmount: number;
  remainingAmount: number;
  percentage: number;
  period: 'Monthly' | 'Quarterly' | 'Yearly';
  startDate: string;
  endDate: string;
  warningThreshold: number;
  isActive: boolean;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  isExceeded: boolean;
  isWarning: boolean;
}

export interface CreateBudgetDto {
  name: string;
  amount: number;
  period: 'Monthly' | 'Quarterly' | 'Yearly';
  startDate: string;
  warningThreshold: number;
  categoryId: string;
}

export interface UpdateBudgetDto {
  id: string;
  name: string;
  amount: number;
  warningThreshold: number;
  isActive: boolean;
}

interface GetBudgetsQuery {
  pageNumber?: number;
  pageSize?: number;
  categoryId?: string;
  period?: string;
  isActive?: boolean;
}

interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export const budgetApi = createApi({
  reducerPath: 'budgetApi',
  baseQuery: fetchBaseQuery({
    baseUrl: BASE_URL,
    prepareHeaders: (headers) => {
      const token = localStorage.getItem('accessToken');
      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ['Budget'],
  endpoints: (builder) => ({
    getBudgets: builder.query<PaginatedResponse<Budget>, GetBudgetsQuery>({
      query: (params) => ({
        url: '/budgets',
        params,
      }),
      providesTags: ['Budget'],
    }),
    getBudgetById: builder.query<Budget, string>({
      query: (id) => `/budgets/${id}`,
      providesTags: ['Budget'],
    }),
    createBudget: builder.mutation<string, CreateBudgetDto>({
      query: (budget) => ({
        url: '/budgets',
        method: 'POST',
        body: budget,
      }),
      invalidatesTags: ['Budget'],
    }),
    updateBudget: builder.mutation<void, UpdateBudgetDto>({
      query: (budget) => ({
        url: `/budgets/${budget.id}`,
        method: 'PUT',
        body: budget,
      }),
      invalidatesTags: ['Budget'],
    }),
    deleteBudget: builder.mutation<void, string>({
      query: (id) => ({
        url: `/budgets/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Budget'],
    }),
  }),
});

export const {
  useGetBudgetsQuery,
  useGetBudgetByIdQuery,
  useCreateBudgetMutation,
  useUpdateBudgetMutation,
  useDeleteBudgetMutation,
} = budgetApi;
