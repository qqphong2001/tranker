export const APP_NAME = 'Expense Management';

export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  REGISTER: '/register',
  DASHBOARD: '/dashboard',
  EXPENSES: '/expenses',
  INCOMES: '/incomes',
  SUBSCRIPTIONS: '/subscriptions',
  BUDGETS: '/budgets',
  CATEGORIES: '/categories',
  TAGS: '/tags',
  RECURRING_TRANSACTIONS: '/recurring-transactions',
  NOTIFICATIONS: '/notifications',
  REPORTS: '/reports',
  SETTINGS: '/settings',
};

export const BUDGET_PERIODS = [
  { value: 1, label: 'Monthly' },
  { value: 2, label: 'Quarterly' },
  { value: 3, label: 'Yearly' },
];

export const DATE_FORMATS = {
  DISPLAY: 'dd/MM/yyyy',
  API: 'yyyy-MM-dd',
};
