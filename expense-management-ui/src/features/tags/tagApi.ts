import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface Tag {
  id: string;
  name: string;
  color?: string;
  createdAt: string;
}

export interface CreateTagDto {
  name: string;
  color?: string;
}

export const tagApi = createApi({
  reducerPath: 'tagApi',
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
  tagTypes: ['Tag'],
  endpoints: (builder) => ({
    getTags: builder.query<Tag[], void>({
      query: () => '/tags',
      providesTags: ['Tag'],
    }),
    createTag: builder.mutation<string, CreateTagDto>({
      query: (tag) => ({
        url: '/tags',
        method: 'POST',
        body: tag,
      }),
      invalidatesTags: ['Tag'],
    }),
    deleteTag: builder.mutation<void, string>({
      query: (id) => ({
        url: `/tags/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Tag'],
    }),
  }),
});

export const {
  useGetTagsQuery,
  useCreateTagMutation,
  useDeleteTagMutation,
} = tagApi;
