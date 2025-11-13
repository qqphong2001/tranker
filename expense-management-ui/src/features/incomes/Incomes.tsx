import React, { useState } from 'react';
import { useGetIncomesQuery, useDeleteIncomeMutation, Income } from './incomeApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import Table, { Column } from '../../components/common/Table';
import Pagination from '../../components/common/Pagination';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import IncomeFormModal from './IncomeFormModal';

const Incomes: React.FC = () => {
  const [page, setPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedIncome, setSelectedIncome] = useState<Income | null>(null);

  const { showToast } = useToast();

  const { data, isLoading, refetch } = useGetIncomesQuery({
    pageNumber: page,
    pageSize: 10,
    searchTerm: searchTerm || undefined,
    categoryId: categoryFilter || undefined,
  });

  const { data: categories } = useGetCategoriesQuery({ transactionType: 'Income' });
  const [deleteIncome] = useDeleteIncomeMutation();

  const handleEdit = (income: Income) => {
    setSelectedIncome(income);
    setIsModalOpen(true);
  };

  const handleDelete = async (income: Income) => {
    if (window.confirm(`Are you sure you want to delete "${income.description}"?`)) {
      try {
        await deleteIncome(income.id).unwrap();
        showToast('Income deleted successfully', 'success');
        refetch();
      } catch (error) {
        showToast('Failed to delete income', 'error');
      }
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedIncome(null);
  };

  const handleSuccess = () => {
    showToast(selectedIncome ? 'Income updated successfully' : 'Income created successfully', 'success');
    handleModalClose();
    refetch();
  };

  const columns: Column<Income>[] = [
    {
      key: 'date',
      header: 'Date',
      render: (income) => new Date(income.date).toLocaleDateString(),
    },
    {
      key: 'description',
      header: 'Description',
    },
    {
      key: 'source',
      header: 'Source',
      render: (income) => income.source || '-',
    },
    {
      key: 'categoryName',
      header: 'Category',
      render: (income) => (
        <span className="inline-flex items-center">
          <span
            className="w-3 h-3 rounded-full mr-2"
            style={{ backgroundColor: income.categoryColor }}
          />
          {income.categoryName}
        </span>
      ),
    },
    {
      key: 'amount',
      header: 'Amount',
      render: (income) => (
        <span className="font-semibold text-green-600 dark:text-green-400">
          +{income.currencySymbol || '$'}{income.amount.toFixed(2)}
        </span>
      ),
    },
  ];

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Incomes</h1>
        <Button onClick={() => setIsModalOpen(true)}>Add Income</Button>
      </div>

      {/* Filters */}
      <div className="mb-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <input
          type="text"
          placeholder="Search incomes..."
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
        <IncomeFormModal
          isOpen={isModalOpen}
          onClose={handleModalClose}
          income={selectedIncome}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default Incomes;
