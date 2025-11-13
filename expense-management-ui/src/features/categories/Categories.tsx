import React, { useState } from 'react';
import { useGetCategoriesQuery, useDeleteCategoryMutation, Category } from './categoryApi';
import Table, { Column } from '../../components/common/Table';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import CategoryFormModal from './CategoryFormModal';

const Categories: React.FC = () => {
  const [transactionTypeFilter, setTransactionTypeFilter] = useState<'Income' | 'Expense' | ''>('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);

  const { showToast } = useToast();

  const { data: categories, isLoading, refetch } = useGetCategoriesQuery(
    transactionTypeFilter ? { transactionType: transactionTypeFilter } : undefined
  );
  const [deleteCategory] = useDeleteCategoryMutation();

  const handleEdit = (category: Category) => {
    if (category.isDefault) {
      showToast('Default categories cannot be edited', 'warning');
      return;
    }
    setSelectedCategory(category);
    setIsModalOpen(true);
  };

  const handleDelete = async (category: Category) => {
    if (category.isDefault) {
      showToast('Default categories cannot be deleted', 'warning');
      return;
    }

    if (window.confirm(`Are you sure you want to delete "${category.name}"?`)) {
      try {
        await deleteCategory(category.id).unwrap();
        showToast('Category deleted successfully', 'success');
        refetch();
      } catch (error: any) {
        showToast(error?.data?.message || 'Failed to delete category', 'error');
      }
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedCategory(null);
  };

  const handleSuccess = () => {
    showToast(selectedCategory ? 'Category updated successfully' : 'Category created successfully', 'success');
    handleModalClose();
    refetch();
  };

  const columns: Column<Category>[] = [
    {
      key: 'icon',
      header: 'Icon',
      render: (category) => (
        <span className="text-2xl">{category.icon || '📁'}</span>
      ),
    },
    {
      key: 'name',
      header: 'Name',
      render: (category) => (
        <div className="flex items-center">
          <span
            className="w-3 h-3 rounded-full mr-2"
            style={{ backgroundColor: category.color }}
          />
          <span className="font-medium">{category.name}</span>
          {category.isDefault && (
            <span className="ml-2 text-xs bg-gray-200 dark:bg-gray-700 text-gray-600 dark:text-gray-400 px-2 py-0.5 rounded">
              Default
            </span>
          )}
        </div>
      ),
    },
    {
      key: 'description',
      header: 'Description',
      render: (category) => (
        <span className="text-gray-600 dark:text-gray-400">
          {category.description || '-'}
        </span>
      ),
    },
    {
      key: 'transactionType',
      header: 'Type',
      render: (category) => (
        <span
          className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
            category.transactionType === 'Income'
              ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'
              : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200'
          }`}
        >
          {category.transactionType}
        </span>
      ),
    },
    {
      key: 'createdAt',
      header: 'Created',
      render: (category) => new Date(category.createdAt).toLocaleDateString(),
    },
  ];

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Categories</h1>
        <Button onClick={() => setIsModalOpen(true)}>Create Category</Button>
      </div>

      {/* Filters */}
      <div className="mb-6">
        <div className="flex space-x-2">
          <button
            onClick={() => setTransactionTypeFilter('')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              transactionTypeFilter === ''
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            All
          </button>
          <button
            onClick={() => setTransactionTypeFilter('Expense')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              transactionTypeFilter === 'Expense'
                ? 'bg-red-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            Expenses
          </button>
          <button
            onClick={() => setTransactionTypeFilter('Income')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              transactionTypeFilter === 'Income'
                ? 'bg-green-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            Incomes
          </button>
        </div>
      </div>

      {/* Table */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <Table
          data={categories || []}
          columns={columns}
          onEdit={handleEdit}
          onDelete={handleDelete}
          loading={isLoading}
        />
      </div>

      {/* Modal */}
      {isModalOpen && (
        <CategoryFormModal
          isOpen={isModalOpen}
          onClose={handleModalClose}
          category={selectedCategory}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default Categories;
