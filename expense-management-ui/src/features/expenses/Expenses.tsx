import React, { useState } from 'react';
import { useGetExpensesQuery, useDeleteExpenseMutation, Expense } from './expenseApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import { useGetTagsQuery } from '../tags/tagApi';
import Table, { Column } from '../../components/common/Table';
import Pagination from '../../components/common/Pagination';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import ExpenseFormModal from './ExpenseFormModal';

const Expenses: React.FC = () => {
  const [page, setPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedExpense, setSelectedExpense] = useState<Expense | null>(null);

  const { showToast } = useToast();

  const { data, isLoading, refetch } = useGetExpensesQuery({
    pageNumber: page,
    pageSize: 10,
    searchTerm: searchTerm || undefined,
    categoryId: categoryFilter || undefined,
  });

  const { data: categories } = useGetCategoriesQuery({ transactionType: 'Expense' });
  const [deleteExpense] = useDeleteExpenseMutation();

  const handleEdit = (expense: Expense) => {
    setSelectedExpense(expense);
    setIsModalOpen(true);
  };

  const handleDelete = async (expense: Expense) => {
    if (window.confirm(`Are you sure you want to delete "${expense.description}"?`)) {
      try {
        await deleteExpense(expense.id).unwrap();
        showToast('Expense deleted successfully', 'success');
        refetch();
      } catch (error) {
        showToast('Failed to delete expense', 'error');
      }
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedExpense(null);
  };

  const handleSuccess = () => {
    showToast(selectedExpense ? 'Expense updated successfully' : 'Expense created successfully', 'success');
    handleModalClose();
    refetch();
  };

  const columns: Column<Expense>[] = [
    {
      key: 'date',
      header: 'Date',
      render: (expense) => new Date(expense.date).toLocaleDateString(),
    },
    {
      key: 'description',
      header: 'Description',
    },
    {
      key: 'categoryName',
      header: 'Category',
      render: (expense) => (
        <span className="inline-flex items-center">
          <span
            className="w-3 h-3 rounded-full mr-2"
            style={{ backgroundColor: expense.categoryColor }}
          />
          {expense.categoryName}
        </span>
      ),
    },
    {
      key: 'amount',
      header: 'Amount',
      render: (expense) => (
        <span className="font-semibold">
          {expense.currencySymbol || '$'}{expense.amount.toFixed(2)}
        </span>
      ),
    },
    {
      key: 'tags',
      header: 'Tags',
      render: (expense) => (
        <div className="flex flex-wrap gap-1">
          {expense.tags?.map((tag) => (
            <span
              key={tag.id}
              className="px-2 py-1 text-xs rounded-full"
              style={{
                backgroundColor: tag.color || '#gray',
                color: 'white',
              }}
            >
              {tag.name}
            </span>
          ))}
        </div>
      ),
    },
  ];

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Expenses</h1>
        <Button onClick={() => setIsModalOpen(true)}>Add Expense</Button>
      </div>

      {/* Filters */}
      <div className="mb-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <input
          type="text"
          placeholder="Search expenses..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
        />
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
      </div>

      {/* Table */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <Table
          data={data?.items || []}
          columns={columns}
          onEdit={handleEdit}
          onDelete={handleDelete}
          loading={isLoading}
        />

        {data && data.totalPages > 1 && (
          <Pagination
            currentPage={data.pageNumber}
            totalPages={data.totalPages}
            hasNextPage={data.hasNextPage}
            hasPreviousPage={data.hasPreviousPage}
            onPageChange={setPage}
          />
        )}
      </div>

      {/* Modal */}
      {isModalOpen && (
        <ExpenseFormModal
          isOpen={isModalOpen}
          onClose={handleModalClose}
          expense={selectedExpense}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default Expenses;
