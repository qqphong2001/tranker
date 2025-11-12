import { Link, useLocation } from 'react-router-dom';
import { ROUTES } from '../../utils/constants';

const navigation = [
  { name: 'Dashboard', href: ROUTES.DASHBOARD, icon: '📊' },
  { name: 'Expenses', href: ROUTES.EXPENSES, icon: '💸' },
  { name: 'Incomes', href: ROUTES.INCOMES, icon: '💰' },
  { name: 'Subscriptions', href: ROUTES.SUBSCRIPTIONS, icon: '🔄' },
  { name: 'Budgets', href: ROUTES.BUDGETS, icon: '🎯' },
  { name: 'Reports', href: ROUTES.REPORTS, icon: '📈' },
  { name: 'Settings', href: ROUTES.SETTINGS, icon: '⚙️' },
];

export default function Sidebar() {
  const location = useLocation();

  return (
    <aside className="hidden md:flex md:flex-shrink-0">
      <div className="flex flex-col w-64 bg-white dark:bg-gray-800 border-r border-gray-200 dark:border-gray-700">
        <div className="flex-1 flex flex-col min-h-0 pt-5 pb-4">
          <div className="flex items-center flex-shrink-0 px-4">
            <h1 className="text-2xl font-bold text-primary-600">💸 ExpenseApp</h1>
          </div>
          <nav className="mt-8 flex-1 px-2 space-y-1">
            {navigation.map((item) => {
              const isActive = location.pathname === item.href;
              return (
                <Link
                  key={item.name}
                  to={item.href}
                  className={`group flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-colors ${
                    isActive
                      ? 'bg-primary-100 dark:bg-primary-900/20 text-primary-700 dark:text-primary-400'
                      : 'text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700'
                  }`}
                >
                  <span className="mr-3 text-xl">{item.icon}</span>
                  {item.name}
                </Link>
              );
            })}
          </nav>
        </div>
      </div>
    </aside>
  );
}
