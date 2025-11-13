import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface Category {
  id: string;
  name: string;
  description?: string;
  icon?: string;
  color?: string;
  transactionType: 'Income' | 'Expense';
  isDefault: boolean;
  createdAt: string;
}

export interface CreateCategoryDto {
  name: string;
  description?: string;
  icon?: string;
  color?: string;
  transactionType: 'Income' | 'Expense';
}

export interface UpdateCategoryDto {
  id: string;
  name: string;
  description?: string;
  icon?: string;
  color?: string;
}

interface GetCategoriesQuery {
  transactionType?: 'Income' | 'Expense';
}

export const categoryApi = createApi({
  reducerPath: 'categoryApi',
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
  tagTypes: ['Category'],
  endpoints: (builder) => ({
    getCategories: builder.query<Category[], GetCategoriesQuery | void>({
      query: (params) => ({
        url: '/categories',
        params,
      }),
      providesTags: ['Category'],
    }),
    createCategory: builder.mutation<string, CreateCategoryDto>({
      query: (category) => ({
        url: '/categories',
        method: 'POST',
        body: category,
      }),
      invalidatesTags: ['Category'],
    }),
    updateCategory: builder.mutation<void, UpdateCategoryDto>({
      query: (category) => ({
        url: `/categories/${category.id}`,
        method: 'PUT',
        body: category,
      }),
      invalidatesTags: ['Category'],
    }),
    deleteCategory: builder.mutation<void, string>({
      query: (id) => ({
        url: `/categories/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Category'],
    }),
  }),
});

export const {
  useGetCategoriesQuery,
  useCreateCategoryMutation,
  useUpdateCategoryMutation,
  useDeleteCategoryMutation,
} = categoryApi;
