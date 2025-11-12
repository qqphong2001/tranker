import { useGetDashboardDataQuery } from './dashboardApi';
import StatCard from './components/StatCard';
import CategoryPieChart from './components/CategoryPieChart';
import IncomeExpenseChart from './components/IncomeExpenseChart';
import { formatCurrency, formatDate } from '../../utils/formatters';

export default function Dashboard() {
  const { data, isLoading, error } = useGetDashboardDataQuery();

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="text-center">
          <p className="text-red-600 dark:text-red-400">Failed to load dashboard data</p>
          <button
            onClick={() => window.location.reload()}
            className="mt-4 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
          >
            Retry
          </button>
        </div>
      </div>
    );
  }

  if (!data) return null;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
        <p className="text-gray-600 dark:text-gray-400">
          Last updated: {new Date().toLocaleString()}
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard
          title="Total Balance"
          value={formatCurrency(data.balance)}
          icon="💰"
          color="bg-gradient-to-br from-blue-400 to-blue-600"
        />
        <StatCard
          title="Monthly Income"
          value={formatCurrency(data.monthlyIncome)}
          icon="📈"
          color="bg-gradient-to-br from-green-400 to-green-600"
        />
        <StatCard
          title="Monthly Expenses"
          value={formatCurrency(data.monthlyExpenses)}
          icon="📉"
          color="bg-gradient-to-br from-red-400 to-red-600"
        />
        <StatCard
          title="Monthly Balance"
          value={formatCurrency(data.monthlyBalance)}
          icon="💵"
          color="bg-gradient-to-br from-purple-400 to-purple-600"
        />
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <IncomeExpenseChart data={data.monthlyTrend} />
        <CategoryPieChart data={data.topExpenseCategories} title="Top Expense Categories" />
      </div>

      {/* Budget Progress */}
      {data.budgetProgress.length > 0 && (
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Budget Progress</h3>
          <div className="space-y-4">
            {data.budgetProgress.map((budget) => (
              <div key={budget.budgetId} className="space-y-2">
                <div className="flex items-center justify-between">
                  <div>
                    <p className="font-medium text-gray-900 dark:text-white">{budget.budgetName}</p>
                    <p className="text-sm text-gray-600 dark:text-gray-400">{budget.categoryName}</p>
                  </div>
                  <div className="text-right">
                    <p className="font-semibold text-gray-900 dark:text-white">
                      {formatCurrency(budget.spentAmount)} / {formatCurrency(budget.budgetAmount)}
                    </p>
                    <p className={`text-sm ${budget.isExceeded ? 'text-red-600' : budget.isWarning ? 'text-yellow-600' : 'text-green-600'}`}>
                      {budget.percentage.toFixed(1)}%
                    </p>
                  </div>
                </div>
                <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-2">
                  <div
                    className={`h-2 rounded-full transition-all ${
                      budget.isExceeded ? 'bg-red-600' : budget.isWarning ? 'bg-yellow-600' : 'bg-green-600'
                    }`}
                    style={{ width: `${Math.min(budget.percentage, 100)}%` }}
                  ></div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Recent Transactions */}
      {data.recentTransactions.length > 0 && (
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Recent Transactions</h3>
          <div className="space-y-3">
            {data.recentTransactions.map((transaction) => (
              <div key={transaction.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700">
                <div className="flex items-center space-x-3">
                  <div
                    className="w-10 h-10 rounded-full flex items-center justify-center text-white font-bold"
                    style={{ backgroundColor: transaction.categoryColor }}
                  >
                    {transaction.categoryName.charAt(0)}
                  </div>
                  <div>
                    <p className="font-medium text-gray-900 dark:text-white">{transaction.description || 'No description'}</p>
                    <p className="text-sm text-gray-600 dark:text-gray-400">
                      {transaction.categoryName} • {formatDate(transaction.date)}
                    </p>
                  </div>
                </div>
                <p className={`font-semibold ${transaction.type === 'Income' ? 'text-green-600' : 'text-red-600'}`}>
                  {transaction.type === 'Income' ? '+' : '-'}{formatCurrency(transaction.amount)}
                </p>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Upcoming Subscriptions */}
      {data.upcomingSubscriptions.length > 0 && (
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Upcoming Subscriptions</h3>
          <div className="space-y-3">
            {data.upcomingSubscriptions.map((subscription) => (
              <div key={subscription.subscriptionId} className="flex items-center justify-between p-3 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700">
                <div>
                  <p className="font-medium text-gray-900 dark:text-white">{subscription.name}</p>
                  <p className="text-sm text-gray-600 dark:text-gray-400">
                    {subscription.categoryName} • Due in {subscription.daysUntilBilling} days
                  </p>
                </div>
                <p className="font-semibold text-gray-900 dark:text-white">
                  {formatCurrency(subscription.amount)}
                </p>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
