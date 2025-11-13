import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface Income {
  id: string;
  amount: number;
  description?: string;
  date: string;
  notes?: string;
  source?: string;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  categoryIcon?: string;
  currencyId?: string;
  currencyCode?: string;
  currencySymbol?: string;
  createdAt: string;
}

export interface CreateIncomeDto {
  amount: number;
  description?: string;
  date: string;
  notes?: string;
  source?: string;
  categoryId: string;
  currencyId?: string;
}

export interface UpdateIncomeDto extends CreateIncomeDto {
  id: string;
}

interface GetIncomesQuery {
  pageNumber?: number;
  pageSize?: number;
  categoryId?: string;
  startDate?: string;
  endDate?: string;
  searchTerm?: string;
}

interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export const incomeApi = createApi({
  reducerPath: 'incomeApi',
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
  tagTypes: ['Income'],
  endpoints: (builder) => ({
    getIncomes: builder.query<PaginatedResponse<Income>, GetIncomesQuery>({
      query: (params) => ({
        url: '/incomes',
        params,
      }),
      providesTags: ['Income'],
    }),
    getIncomeById: builder.query<Income, string>({
      query: (id) => `/incomes/${id}`,
      providesTags: ['Income'],
    }),
    createIncome: builder.mutation<string, CreateIncomeDto>({
      query: (income) => ({
        url: '/incomes',
        method: 'POST',
        body: income,
      }),
      invalidatesTags: ['Income'],
    }),
    updateIncome: builder.mutation<void, UpdateIncomeDto>({
      query: (income) => ({
        url: `/incomes/${income.id}`,
        method: 'PUT',
        body: income,
      }),
      invalidatesTags: ['Income'],
    }),
    deleteIncome: builder.mutation<void, string>({
      query: (id) => ({
        url: `/incomes/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Income'],
    }),
  }),
});

export const {
  useGetIncomesQuery,
  useGetIncomeByIdQuery,
  useCreateIncomeMutation,
  useUpdateIncomeMutation,
  useDeleteIncomeMutation,
} = incomeApi;
