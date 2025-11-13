import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export type RecurrenceType = 'None' | 'Daily' | 'Weekly' | 'Monthly' | 'Yearly';
export type TransactionType = 'Income' | 'Expense';

export interface RecurringTransaction {
  id: string;
  name: string;
  amount: number;
  description?: string;
  recurrenceType: RecurrenceType;
  transactionType: TransactionType;
  startDate: string;
  endDate?: string;
  nextOccurrence: string;
  isActive: boolean;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  currencyId?: string;
  currencyCode?: string;
  currencySymbol?: string;
}

export interface CreateRecurringTransactionDto {
  name: string;
  amount: number;
  description?: string;
  recurrenceType: RecurrenceType;
  transactionType: TransactionType;
  startDate: string;
  endDate?: string;
  categoryId: string;
  currencyId?: string;
}

export interface UpdateRecurringTransactionDto {
  id: string;
  name: string;
  amount: number;
  description?: string;
  recurrenceType: RecurrenceType;
  isActive: boolean;
  categoryId: string;
}

interface GetRecurringTransactionsQuery {
  transactionType?: TransactionType;
  isActive?: boolean;
}

export const recurringTransactionApi = createApi({
  reducerPath: 'recurringTransactionApi',
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
  tagTypes: ['RecurringTransaction'],
  endpoints: (builder) => ({
    getRecurringTransactions: builder.query<RecurringTransaction[], GetRecurringTransactionsQuery | void>({
      query: (params) => ({
        url: '/recurringtransactions',
        params,
      }),
      providesTags: ['RecurringTransaction'],
    }),
    createRecurringTransaction: builder.mutation<string, CreateRecurringTransactionDto>({
      query: (transaction) => ({
        url: '/recurringtransactions',
        method: 'POST',
        body: transaction,
      }),
      invalidatesTags: ['RecurringTransaction'],
    }),
    updateRecurringTransaction: builder.mutation<void, UpdateRecurringTransactionDto>({
      query: (transaction) => ({
        url: `/recurringtransactions/${transaction.id}`,
        method: 'PUT',
        body: transaction,
      }),
      invalidatesTags: ['RecurringTransaction'],
    }),
    deleteRecurringTransaction: builder.mutation<void, string>({
      query: (id) => ({
        url: `/recurringtransactions/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['RecurringTransaction'],
    }),
  }),
});

export const {
  useGetRecurringTransactionsQuery,
  useCreateRecurringTransactionMutation,
  useUpdateRecurringTransactionMutation,
  useDeleteRecurringTransactionMutation,
} = recurringTransactionApi;
