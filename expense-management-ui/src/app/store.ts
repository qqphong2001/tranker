import { configureStore } from '@reduxjs/toolkit';
import authReducer from '../features/auth/authSlice';
import { dashboardApi } from '../features/dashboard/dashboardApi';
import { expenseApi } from '../features/expenses/expenseApi';
import { incomeApi } from '../features/incomes/incomeApi';
import { budgetApi } from '../features/budgets/budgetApi';
import { subscriptionApi } from '../features/subscriptions/subscriptionApi';
import { categoryApi } from '../features/categories/categoryApi';
import { tagApi } from '../features/tags/tagApi';
import { notificationApi } from '../features/notifications/notificationApi';
import { recurringTransactionApi } from '../features/recurringTransactions/recurringTransactionApi';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    [dashboardApi.reducerPath]: dashboardApi.reducer,
    [expenseApi.reducerPath]: expenseApi.reducer,
    [incomeApi.reducerPath]: incomeApi.reducer,
    [budgetApi.reducerPath]: budgetApi.reducer,
    [subscriptionApi.reducerPath]: subscriptionApi.reducer,
    [categoryApi.reducerPath]: categoryApi.reducer,
    [tagApi.reducerPath]: tagApi.reducer,
    [notificationApi.reducerPath]: notificationApi.reducer,
    [recurringTransactionApi.reducerPath]: recurringTransactionApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware()
      .concat(dashboardApi.middleware)
      .concat(expenseApi.middleware)
      .concat(incomeApi.middleware)
      .concat(budgetApi.middleware)
      .concat(subscriptionApi.middleware)
      .concat(categoryApi.middleware)
      .concat(tagApi.middleware)
      .concat(notificationApi.middleware)
      .concat(recurringTransactionApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
