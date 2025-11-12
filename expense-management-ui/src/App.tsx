import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAppSelector } from './app/hooks';
import Layout from './components/layout/Layout';
import Login from './features/auth/Login';
import Register from './features/auth/Register';
import Dashboard from './features/dashboard/Dashboard';
import { ROUTES } from './utils/constants';

function PrivateRoute({ children }: { children: JSX.Element }) {
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
  return isAuthenticated ? children : <Navigate to={ROUTES.LOGIN} />;
}

function App() {
  return (
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
          <Route
            path={ROUTES.EXPENSES}
            element={
              <div className="text-center py-12">
                <p className="text-2xl font-bold text-gray-900 dark:text-white">Expenses Page</p>
                <p className="text-gray-600 dark:text-gray-400 mt-2">Coming soon...</p>
              </div>
            }
          />
          <Route
            path={ROUTES.INCOMES}
            element={
              <div className="text-center py-12">
                <p className="text-2xl font-bold text-gray-900 dark:text-white">Incomes Page</p>
                <p className="text-gray-600 dark:text-gray-400 mt-2">Coming soon...</p>
              </div>
            }
          />
          <Route
            path={ROUTES.SUBSCRIPTIONS}
            element={
              <div className="text-center py-12">
                <p className="text-2xl font-bold text-gray-900 dark:text-white">Subscriptions Page</p>
                <p className="text-gray-600 dark:text-gray-400 mt-2">Coming soon...</p>
              </div>
            }
          />
          <Route
            path={ROUTES.BUDGETS}
            element={
              <div className="text-center py-12">
                <p className="text-2xl font-bold text-gray-900 dark:text-white">Budgets Page</p>
                <p className="text-gray-600 dark:text-gray-400 mt-2">Coming soon...</p>
              </div>
            }
          />
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
  );
}

export default App;
