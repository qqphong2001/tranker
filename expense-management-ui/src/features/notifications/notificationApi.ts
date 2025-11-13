import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface Notification {
  id: string;
  title: string;
  message: string;
  type: 'BudgetExceeded' | 'BudgetWarning' | 'SubscriptionDue' | 'RecurringTransactionCreated' | 'General';
  isRead: boolean;
  readAt?: string;
  relatedEntityId?: string;
  createdAt: string;
}

interface GetNotificationsQuery {
  pageNumber?: number;
  pageSize?: number;
  isRead?: boolean;
}

interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export const notificationApi = createApi({
  reducerPath: 'notificationApi',
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
  tagTypes: ['Notification'],
  endpoints: (builder) => ({
    getNotifications: builder.query<PaginatedResponse<Notification>, GetNotificationsQuery>({
      query: (params) => ({
        url: '/notifications',
        params,
      }),
      providesTags: ['Notification'],
    }),
    getUnreadCount: builder.query<{ count: number }, void>({
      query: () => '/notifications/unread-count',
      providesTags: ['Notification'],
    }),
    markAsRead: builder.mutation<void, string>({
      query: (id) => ({
        url: `/notifications/${id}/mark-as-read`,
        method: 'PUT',
      }),
      invalidatesTags: ['Notification'],
    }),
    markAllAsRead: builder.mutation<void, void>({
      query: () => ({
        url: '/notifications/mark-all-as-read',
        method: 'PUT',
      }),
      invalidatesTags: ['Notification'],
    }),
    deleteNotification: builder.mutation<void, string>({
      query: (id) => ({
        url: `/notifications/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Notification'],
    }),
  }),
});

export const {
  useGetNotificationsQuery,
  useGetUnreadCountQuery,
  useMarkAsReadMutation,
  useMarkAllAsReadMutation,
  useDeleteNotificationMutation,
} = notificationApi;
