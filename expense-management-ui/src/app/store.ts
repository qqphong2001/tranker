import { configureStore } from '@reduxjs/toolkit';
import authReducer from '../features/auth/authSlice';
import { dashboardApi } from '../features/dashboard/dashboardApi';
import { expenseApi } from '../features/expenses/expenseApi';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    [dashboardApi.reducerPath]: dashboardApi.reducer,
    [expenseApi.reducerPath]: expenseApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware()
      .concat(dashboardApi.middleware)
      .concat(expenseApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
