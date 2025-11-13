import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAppSelector } from './app/hooks';
import Layout from './components/layout/Layout';
import Login from './features/auth/Login';
import Register from './features/auth/Register';
import Dashboard from './features/dashboard/Dashboard';
import Expenses from './features/expenses/Expenses';
import Incomes from './features/incomes/Incomes';
import Budgets from './features/budgets/Budgets';
import Subscriptions from './features/subscriptions/Subscriptions';
import Categories from './features/categories/Categories';
import Tags from './features/tags/Tags';
import RecurringTransactions from './features/recurringTransactions/RecurringTransactions';
import Notifications from './features/notifications/Notifications';
import { ToastProvider } from './components/common/ToastContainer';
import { ROUTES } from './utils/constants';

function PrivateRoute({ children }: { children: JSX.Element }) {
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
  return isAuthenticated ? children : <Navigate to={ROUTES.LOGIN} />;
}

function App() {
  return (
    <ToastProvider>
      <BrowserRouter>
        <Routes>
          <Route path={ROUTES.LOGIN} element={<Login />} />
          <Route path={ROUTES.REGISTER} element={<Register />} />
          <Route
            path="/"
            element={
              <PrivateRoute>
                <Layout />
              </PrivateRoute>
            }
          >
            <Route index element={<Navigate to={ROUTES.DASHBOARD} />} />
            <Route path={ROUTES.DASHBOARD} element={<Dashboard />} />
            <Route path={ROUTES.EXPENSES} element={<Expenses />} />
            <Route path={ROUTES.INCOMES} element={<Incomes />} />
            <Route path={ROUTES.BUDGETS} element={<Budgets />} />
            <Route path={ROUTES.SUBSCRIPTIONS} element={<Subscriptions />} />
            <Route path={ROUTES.CATEGORIES} element={<Categories />} />
            <Route path={ROUTES.TAGS} element={<Tags />} />
            <Route path={ROUTES.RECURRING_TRANSACTIONS} element={<RecurringTransactions />} />
            <Route path={ROUTES.NOTIFICATIONS} element={<Notifications />} />
            <Route
              path={ROUTES.REPORTS}
              element={
                <div className="text-center py-12">
                  <p className="text-2xl font-bold text-gray-900 dark:text-white">Reports Page</p>
                  <p className="text-gray-600 dark:text-gray-400 mt-2">Coming soon...</p>
                </div>
              }
            />
            <Route
              path={ROUTES.SETTINGS}
              element={
                <div className="text-center py-12">
                  <p className="text-2xl font-bold text-gray-900 dark:text-white">Settings Page</p>
                  <p className="text-gray-600 dark:text-gray-400 mt-2">Coming soon...</p>
                </div>
              }
            />
          </Route>
        </Routes>
      </BrowserRouter>
    </ToastProvider>
  );
}

export default App;
