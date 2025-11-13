import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface Subscription {
  id: string;
  name: string;
  description?: string;
  amount: number;
  startDate: string;
  endDate?: string;
  nextBillingDate: string;
  billingCycle: number;
  isActive: boolean;
  reminderDaysBefore: number;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  currencyId?: string;
  currencyCode?: string;
  currencySymbol?: string;
}

export interface CreateSubscriptionDto {
  name: string;
  description?: string;
  amount: number;
  startDate: string;
  endDate?: string;
  billingCycle: number;
  reminderDaysBefore: number;
  categoryId: string;
  currencyId?: string;
}

export interface UpdateSubscriptionDto {
  id: string;
  name: string;
  description?: string;
  amount: number;
  billingCycle: number;
  reminderDaysBefore: number;
  isActive: boolean;
  categoryId: string;
}

interface GetSubscriptionsQuery {
  pageNumber?: number;
  pageSize?: number;
  categoryId?: string;
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

export const subscriptionApi = createApi({
  reducerPath: 'subscriptionApi',
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
  tagTypes: ['Subscription'],
  endpoints: (builder) => ({
    getSubscriptions: builder.query<PaginatedResponse<Subscription>, GetSubscriptionsQuery>({
      query: (params) => ({
        url: '/subscriptions',
        params,
      }),
      providesTags: ['Subscription'],
    }),
    getSubscriptionById: builder.query<Subscription, string>({
      query: (id) => `/subscriptions/${id}`,
      providesTags: ['Subscription'],
    }),
    createSubscription: builder.mutation<string, CreateSubscriptionDto>({
      query: (subscription) => ({
        url: '/subscriptions',
        method: 'POST',
        body: subscription,
      }),
      invalidatesTags: ['Subscription'],
    }),
    updateSubscription: builder.mutation<void, UpdateSubscriptionDto>({
      query: (subscription) => ({
        url: `/subscriptions/${subscription.id}`,
        method: 'PUT',
        body: subscription,
      }),
      invalidatesTags: ['Subscription'],
    }),
    deleteSubscription: builder.mutation<void, string>({
      query: (id) => ({
        url: `/subscriptions/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Subscription'],
    }),
  }),
});

export const {
  useGetSubscriptionsQuery,
  useGetSubscriptionByIdQuery,
  useCreateSubscriptionMutation,
  useUpdateSubscriptionMutation,
  useDeleteSubscriptionMutation,
} = subscriptionApi;
