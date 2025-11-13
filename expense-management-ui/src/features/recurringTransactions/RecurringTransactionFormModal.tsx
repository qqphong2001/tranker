import React, { useState, useEffect } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import FormSelect from '../../components/common/FormSelect';
import Button from '../../components/common/Button';
import { useCreateRecurringTransactionMutation, useUpdateRecurringTransactionMutation, RecurringTransaction, CreateRecurringTransactionDto, UpdateRecurringTransactionDto, RecurrenceType, TransactionType } from './recurringTransactionApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import { useToast } from '../../components/common/ToastContainer';

interface RecurringTransactionFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  transaction?: RecurringTransaction | null;
  onSuccess: () => void;
}

const RecurringTransactionFormModal: React.FC<RecurringTransactionFormModalProps> = ({ isOpen, onClose, transaction, onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    amount: '',
    description: '',
    recurrenceType: 'Monthly' as RecurrenceType,
    transactionType: 'Expense' as TransactionType,
    startDate: new Date().toISOString().split('T')[0],
    endDate: '',
    categoryId: '',
    isActive: true,
  });

  const { showToast } = useToast();
  const [createTransaction, { isLoading: creating }] = useCreateRecurringTransactionMutation();
  const [updateTransaction, { isLoading: updating }] = useUpdateRecurringTransactionMutation();

  // Get categories based on transaction type
  const { data: categories, isLoading: categoriesLoading } = useGetCategoriesQuery({
    transactionType: formData.transactionType,
  });

  useEffect(() => {
    if (transaction) {
      setFormData({
        name: transaction.name,
        amount: transaction.amount.toString(),
        description: transaction.description || '',
        recurrenceType: transaction.recurrenceType,
        transactionType: transaction.transactionType,
        startDate: transaction.startDate.split('T')[0],
        endDate: transaction.endDate ? transaction.endDate.split('T')[0] : '',
        categoryId: transaction.categoryId,
        isActive: transaction.isActive,
      });
    } else {
      setFormData({
        name: '',
        amount: '',
        description: '',
        recurrenceType: 'Monthly',
        transactionType: 'Expense',
        startDate: new Date().toISOString().split('T')[0],
        endDate: '',
        categoryId: '',
        isActive: true,
      });
    }
  }, [transaction, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    if (type === 'checkbox') {
      setFormData((prev) => ({ ...prev, [name]: (e.target as HTMLInputElement).checked }));
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.categoryId) {
      showToast('Please select a category', 'error');
      return;
    }

    try {
      if (transaction) {
        const updateDto: UpdateRecurringTransactionDto = {
          id: transaction.id,
          name: formData.name,
          amount: parseFloat(formData.amount),
          description: formData.description,
          recurrenceType: formData.recurrenceType,
          isActive: formData.isActive,
          categoryId: formData.categoryId,
        };
        await updateTransaction(updateDto).unwrap();
      } else {
        const createDto: CreateRecurringTransactionDto = {
          name: formData.name,
          amount: parseFloat(formData.amount),
          description: formData.description,
          recurrenceType: formData.recurrenceType,
          transactionType: formData.transactionType,
          startDate: formData.startDate,
          endDate: formData.endDate || undefined,
          categoryId: formData.categoryId,
        };
        await createTransaction(createDto).unwrap();
      }
      onSuccess();
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to save recurring transaction', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={transaction ? 'Edit Recurring Transaction' : 'Create Recurring Transaction'} size="lg">
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormInput
            label="Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="e.g., Monthly Rent"
            required
          />

          <FormInput
            label="Amount"
            name="amount"
            type="number"
            step="0.01"
            value={formData.amount}
            onChange={handleChange}
            required
          />

          <FormSelect
            label="Transaction Type"
            name="transactionType"
            value={formData.transactionType}
            onChange={handleChange}
            options={[
              { value: 'Expense', label: 'Expense' },
              { value: 'Income', label: 'Income' },
            ]}
            required
            disabled={!!transaction}
          />

          <FormSelect
            label="Recurrence"
            name="recurrenceType"
            value={formData.recurrenceType}
            onChange={handleChange}
            options={[
              { value: 'Daily', label: 'Daily' },
              { value: 'Weekly', label: 'Weekly' },
              { value: 'Monthly', label: 'Monthly' },
              { value: 'Yearly', label: 'Yearly' },
            ]}
            required
          />

          {!transaction && (
            <>
              <FormInput
                label="Start Date"
                name="startDate"
                type="date"
                value={formData.startDate}
                onChange={handleChange}
                required
              />

              <FormInput
                label="End Date (Optional)"
                name="endDate"
                type="date"
                value={formData.endDate}
                onChange={handleChange}
              />
            </>
          )}

          <FormSelect
            label="Category"
            name="categoryId"
            value={formData.categoryId}
            onChange={handleChange}
            options={
              categories?.map((cat) => ({
                value: cat.id,
                label: cat.name,
              })) || []
            }
            required
            disabled={categoriesLoading}
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Description
          </label>
          <textarea
            name="description"
            value={formData.description}
            onChange={handleChange}
            rows={3}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            placeholder="Transaction details..."
          />
        </div>

        {transaction && (
          <div className="mb-4">
            <label className="flex items-center space-x-2 cursor-pointer">
              <input
                type="checkbox"
                name="isActive"
                checked={formData.isActive}
                onChange={handleChange}
                className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
              />
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300">
                Active
              </span>
            </label>
          </div>
        )}

        {/* Actions */}
        <div className="flex justify-end space-x-2 mt-6">
          <Button type="button" variant="secondary" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" disabled={creating || updating}>
            {creating || updating ? 'Saving...' : transaction ? 'Update' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default RecurringTransactionFormModal;
