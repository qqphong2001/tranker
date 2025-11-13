import React, { useState } from 'react';
import { useGetRecurringTransactionsQuery, useDeleteRecurringTransactionMutation, RecurringTransaction, TransactionType } from './recurringTransactionApi';
import Table, { Column } from '../../components/common/Table';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import RecurringTransactionFormModal from './RecurringTransactionFormModal';

const RecurringTransactions: React.FC = () => {
  const [transactionTypeFilter, setTransactionTypeFilter] = useState<TransactionType | ''>('');
  const [activeFilter, setActiveFilter] = useState<boolean | undefined>(undefined);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState<RecurringTransaction | null>(null);

  const { showToast } = useToast();

  const { data: transactions, isLoading, refetch } = useGetRecurringTransactionsQuery(
    transactionTypeFilter || activeFilter !== undefined
      ? {
          transactionType: transactionTypeFilter || undefined,
          isActive: activeFilter,
        }
      : undefined
  );

  const [deleteTransaction] = useDeleteRecurringTransactionMutation();

  const handleEdit = (transaction: RecurringTransaction) => {
    setSelectedTransaction(transaction);
    setIsModalOpen(true);
  };

  const handleDelete = async (transaction: RecurringTransaction) => {
    if (window.confirm(`Are you sure you want to delete "${transaction.name}"?`)) {
      try {
        await deleteTransaction(transaction.id).unwrap();
        showToast('Recurring transaction deleted successfully', 'success');
        refetch();
      } catch (error) {
        showToast('Failed to delete recurring transaction', 'error');
      }
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedTransaction(null);
  };

  const handleSuccess = () => {
    showToast(selectedTransaction ? 'Recurring transaction updated successfully' : 'Recurring transaction created successfully', 'success');
    handleModalClose();
    refetch();
  };

  const columns: Column<RecurringTransaction>[] = [
    {
      key: 'name',
      header: 'Name',
      render: (transaction) => (
        <div>
          <div className="font-medium text-gray-900 dark:text-white">{transaction.name}</div>
          {transaction.description && (
            <div className="text-sm text-gray-500 dark:text-gray-400">{transaction.description}</div>
          )}
        </div>
      ),
    },
    {
      key: 'transactionType',
      header: 'Type',
      render: (transaction) => (
        <span
          className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
            transaction.transactionType === 'Income'
              ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'
              : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200'
          }`}
        >
          {transaction.transactionType}
        </span>
      ),
    },
    {
      key: 'amount',
      header: 'Amount',
      render: (transaction) => (
        <span
          className={`font-semibold ${
            transaction.transactionType === 'Income'
              ? 'text-green-600 dark:text-green-400'
              : 'text-red-600 dark:text-red-400'
          }`}
        >
          {transaction.transactionType === 'Income' ? '+' : '-'}
          {transaction.currencySymbol || '$'}{transaction.amount.toFixed(2)}
        </span>
      ),
    },
    {
      key: 'recurrenceType',
      header: 'Frequency',
      render: (transaction) => (
        <span className="text-gray-700 dark:text-gray-300">{transaction.recurrenceType}</span>
      ),
    },
    {
      key: 'categoryName',
      header: 'Category',
      render: (transaction) => (
        <span className="inline-flex items-center">
          <span
            className="w-3 h-3 rounded-full mr-2"
            style={{ backgroundColor: transaction.categoryColor }}
          />
          {transaction.categoryName}
        </span>
      ),
    },
    {
      key: 'nextOccurrence',
      header: 'Next Date',
      render: (transaction) => (
        <div>
          <div className="text-gray-900 dark:text-white">
            {new Date(transaction.nextOccurrence).toLocaleDateString()}
          </div>
          <div className="text-xs text-gray-500 dark:text-gray-400">
            {transaction.isActive ? (
              <span className="text-green-600 dark:text-green-400">Active</span>
            ) : (
              <span className="text-gray-500 dark:text-gray-400">Inactive</span>
            )}
          </div>
        </div>
      ),
    },
  ];

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Recurring Transactions</h1>
        <Button onClick={() => setIsModalOpen(true)}>Create Recurring Transaction</Button>
      </div>

      {/* Filters */}
      <div className="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="flex space-x-2">
          <button
            onClick={() => setTransactionTypeFilter('')}
            className={`flex-1 px-4 py-2 rounded-lg font-medium transition-colors ${
              transactionTypeFilter === ''
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            All
          </button>
          <button
            onClick={() => setTransactionTypeFilter('Expense')}
            className={`flex-1 px-4 py-2 rounded-lg font-medium transition-colors ${
              transactionTypeFilter === 'Expense'
                ? 'bg-red-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            Expenses
          </button>
          <button
            onClick={() => setTransactionTypeFilter('Income')}
            className={`flex-1 px-4 py-2 rounded-lg font-medium transition-colors ${
              transactionTypeFilter === 'Income'
                ? 'bg-green-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            Incomes
          </button>
        </div>
        <select
          value={activeFilter === undefined ? '' : activeFilter.toString()}
          onChange={(e) => setActiveFilter(e.target.value === '' ? undefined : e.target.value === 'true')}
          className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
        >
          <option value="">All Status</option>
          <option value="true">Active</option>
          <option value="false">Inactive</option>
        </select>
      </div>

      {/* Table */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <Table
          data={transactions || []}
          columns={columns}
          onEdit={handleEdit}
          onDelete={handleDelete}
          loading={isLoading}
        />
      </div>

      {/* Modal */}
      {isModalOpen && (
        <RecurringTransactionFormModal
          isOpen={isModalOpen}
          onClose={handleModalClose}
          transaction={selectedTransaction}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default RecurringTransactions;
