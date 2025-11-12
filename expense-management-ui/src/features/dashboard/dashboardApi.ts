import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface DashboardData {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
  monthlyIncome: number;
  monthlyExpenses: number;
  monthlyBalance: number;
  topExpenseCategories: CategorySummary[];
  topIncomeCategories: CategorySummary[];
  monthlyTrend: MonthlyTrend[];
  recentTransactions: RecentTransaction[];
  budgetProgress: BudgetProgress[];
  upcomingSubscriptions: UpcomingSubscription[];
}

export interface CategorySummary {
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  categoryIcon?: string;
  amount: number;
  count: number;
  percentage: number;
}

export interface MonthlyTrend {
  year: number;
  month: number;
  monthName: string;
  income: number;
  expenses: number;
  balance: number;
}

export interface RecentTransaction {
  id: string;
  type: string;
  amount: number;
  description?: string;
  date: string;
  categoryName: string;
  categoryColor: string;
}

export interface BudgetProgress {
  budgetId: string;
  budgetName: string;
  categoryName: string;
  budgetAmount: number;
  spentAmount: number;
  remainingAmount: number;
  percentage: number;
  isExceeded: boolean;
  isWarning: boolean;
}

export interface UpcomingSubscription {
  subscriptionId: string;
  name: string;
  amount: number;
  nextBillingDate: string;
  daysUntilBilling: number;
  categoryName: string;
}

export const dashboardApi = createApi({
  reducerPath: 'dashboardApi',
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
  endpoints: (builder) => ({
    getDashboardData: builder.query<DashboardData, void>({
      query: () => '/dashboard',
    }),
  }),
});

export const { useGetDashboardDataQuery } = dashboardApi;
