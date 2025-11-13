import React, { useState } from 'react';
import { useGetBudgetsQuery, useDeleteBudgetMutation, Budget } from './budgetApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import Pagination from '../../components/common/Pagination';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import BudgetFormModal from './BudgetFormModal';

const Budgets: React.FC = () => {
  const [page, setPage] = useState(1);
  const [categoryFilter, setCategoryFilter] = useState('');
  const [periodFilter, setPeriodFilter] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedBudget, setSelectedBudget] = useState<Budget | null>(null);

  const { showToast } = useToast();

  const { data, isLoading, refetch } = useGetBudgetsQuery({
    pageNumber: page,
    pageSize: 10,
    categoryId: categoryFilter || undefined,
    period: periodFilter || undefined,
  });

  const { data: categories } = useGetCategoriesQuery({ transactionType: 'Expense' });
  const [deleteBudget] = useDeleteBudgetMutation();

  const handleEdit = (budget: Budget) => {
    setSelectedBudget(budget);
    setIsModalOpen(true);
  };

  const handleDelete = async (budget: Budget) => {
    if (window.confirm(`Are you sure you want to delete "${budget.name}"?`)) {
      try {
        await deleteBudget(budget.id).unwrap();
        showToast('Budget deleted successfully', 'success');
        refetch();
      } catch (error) {
        showToast('Failed to delete budget', 'error');
      }
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedBudget(null);
  };

  const handleSuccess = () => {
    showToast(selectedBudget ? 'Budget updated successfully' : 'Budget created successfully', 'success');
    handleModalClose();
    refetch();
  };

  const getProgressColor = (budget: Budget) => {
    if (budget.isExceeded) return 'bg-red-500';
    if (budget.isWarning) return 'bg-yellow-500';
    return 'bg-green-500';
  };

  const getStatusBadge = (budget: Budget) => {
    if (!budget.isActive) {
      return (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-gray-200 text-gray-700 dark:bg-gray-700 dark:text-gray-300">
          Inactive
        </span>
      );
    }
    if (budget.isExceeded) {
      return (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200">
          Exceeded
        </span>
      );
    }
    if (budget.isWarning) {
      return (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200">
          Warning
        </span>
      );
    }
    return (
      <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200">
        On Track
      </span>
    );
  };

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Budgets</h1>
        <Button onClick={() => setIsModalOpen(true)}>Create Budget</Button>
      </div>

      {/* Filters */}
      <div className="mb-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <select
          value={categoryFilter}
          onChange={(e) => setCategoryFilter(e.target.value)}
          className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
        >
          <option value="">All Categories</option>
          {categories?.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        <select
          value={periodFilter}
          onChange={(e) => setPeriodFilter(e.target.value)}
          className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
        >
          <option value="">All Periods</option>
          <option value="Monthly">Monthly</option>
          <option value="Quarterly">Quarterly</option>
          <option value="Yearly">Yearly</option>
        </select>
      </div>

      {/* Budget Cards */}
      {isLoading ? (
        <div className="text-center py-12 text-gray-500 dark:text-gray-400">Loading...</div>
      ) : data?.items.length === 0 ? (
        <div className="text-center py-12 text-gray-500 dark:text-gray-400">No budgets found</div>
      ) : (
        <div className="space-y-4">
          {data?.items.map((budget) => (
            <div
              key={budget.id}
              className="bg-white dark:bg-gray-800 rounded-lg shadow p-6"
            >
              <div className="flex justify-between items-start mb-4">
                <div className="flex-1">
                  <div className="flex items-center space-x-3">
                    <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                      {budget.name}
                    </h3>
                    {getStatusBadge(budget)}
                  </div>
                  <div className="flex items-center mt-2 space-x-2">
                    <span
                      className="w-3 h-3 rounded-full"
                      style={{ backgroundColor: budget.categoryColor }}
                    />
                    <span className="text-sm text-gray-600 dark:text-gray-400">
                      {budget.categoryName}
                    </span>
                    <span className="text-sm text-gray-400 dark:text-gray-500">•</span>
                    <span className="text-sm text-gray-600 dark:text-gray-400">
                      {budget.period}
                    </span>
                  </div>
                </div>
                <div className="flex space-x-2">
                  <button
                    onClick={() => handleEdit(budget)}
                    className="text-blue-600 dark:text-blue-400 hover:text-blue-800 dark:hover:text-blue-300"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(budget)}
                    className="text-red-600 dark:text-red-400 hover:text-red-800 dark:hover:text-red-300"
                  >
                    Delete
                  </button>
                </div>
              </div>

              {/* Progress Bar */}
              <div className="mb-2">
                <div className="flex justify-between text-sm text-gray-600 dark:text-gray-400 mb-1">
                  <span>
                    ${budget.spentAmount.toFixed(2)} of ${budget.amount.toFixed(2)}
                  </span>
                  <span>{budget.percentage.toFixed(1)}%</span>
                </div>
                <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-4">
                  <div
                    className={`${getProgressColor(budget)} h-4 rounded-full transition-all duration-300`}
                    style={{ width: `${Math.min(budget.percentage, 100)}%` }}
                  />
                </div>
              </div>

              {/* Budget Info */}
              <div className="flex justify-between items-center text-sm">
                <span className="text-gray-600 dark:text-gray-400">
                  Remaining: ${budget.remainingAmount.toFixed(2)}
                </span>
                <span className="text-gray-500 dark:text-gray-500">
                  {new Date(budget.startDate).toLocaleDateString()} - {new Date(budget.endDate).toLocaleDateString()}
                </span>
              </div>
            </div>
          ))}
        </div>
      )}

      {data && data.totalPages > 1 && (
        <div className="mt-6">
          <Pagination
            currentPage={data.pageNumber}
            totalPages={data.totalPages}
            hasNextPage={data.hasNextPage}
            hasPreviousPage={data.hasPreviousPage}
            onPageChange={setPage}
          />
        </div>
      )}

      {/* Modal */}
      {isModalOpen && (
        <BudgetFormModal
          isOpen={isModalOpen}
          onClose={handleModalClose}
          budget={selectedBudget}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default Budgets;
