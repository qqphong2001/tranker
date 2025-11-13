import React, { useState, useEffect } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import FormSelect from '../../components/common/FormSelect';
import Button from '../../components/common/Button';
import { useCreateBudgetMutation, useUpdateBudgetMutation, Budget, CreateBudgetDto, UpdateBudgetDto } from './budgetApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import { useToast } from '../../components/common/ToastContainer';

interface BudgetFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  budget?: Budget | null;
  onSuccess: () => void;
}

const BudgetFormModal: React.FC<BudgetFormModalProps> = ({ isOpen, onClose, budget, onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    amount: '',
    period: 'Monthly' as 'Monthly' | 'Quarterly' | 'Yearly',
    startDate: new Date().toISOString().split('T')[0],
    warningThreshold: '80',
    categoryId: '',
    isActive: true,
  });

  const { showToast } = useToast();
  const { data: categories, isLoading: categoriesLoading } = useGetCategoriesQuery({ transactionType: 'Expense' });
  const [createBudget, { isLoading: creating }] = useCreateBudgetMutation();
  const [updateBudget, { isLoading: updating }] = useUpdateBudgetMutation();

  useEffect(() => {
    if (budget) {
      setFormData({
        name: budget.name,
        amount: budget.amount.toString(),
        period: budget.period,
        startDate: budget.startDate.split('T')[0],
        warningThreshold: budget.warningThreshold.toString(),
        categoryId: budget.categoryId,
        isActive: budget.isActive,
      });
    } else {
      setFormData({
        name: '',
        amount: '',
        period: 'Monthly',
        startDate: new Date().toISOString().split('T')[0],
        warningThreshold: '80',
        categoryId: '',
        isActive: true,
      });
    }
  }, [budget, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
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
      if (budget) {
        const updateDto: UpdateBudgetDto = {
          id: budget.id,
          name: formData.name,
          amount: parseFloat(formData.amount),
          warningThreshold: parseFloat(formData.warningThreshold),
          isActive: formData.isActive,
        };
        await updateBudget(updateDto).unwrap();
      } else {
        const createDto: CreateBudgetDto = {
          name: formData.name,
          amount: parseFloat(formData.amount),
          period: formData.period,
          startDate: formData.startDate,
          warningThreshold: parseFloat(formData.warningThreshold),
          categoryId: formData.categoryId,
        };
        await createBudget(createDto).unwrap();
      }
      onSuccess();
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to save budget', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={budget ? 'Edit Budget' : 'Create Budget'} size="lg">
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormInput
            label="Budget Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="e.g., Monthly Groceries"
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

          {!budget && (
            <>
              <FormSelect
                label="Period"
                name="period"
                value={formData.period}
                onChange={handleChange}
                options={[
                  { value: 'Monthly', label: 'Monthly' },
                  { value: 'Quarterly', label: 'Quarterly' },
                  { value: 'Yearly', label: 'Yearly' },
                ]}
                required
              />

              <FormInput
                label="Start Date"
                name="startDate"
                type="date"
                value={formData.startDate}
                onChange={handleChange}
                required
              />

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
            </>
          )}

          <FormInput
            label="Warning Threshold (%)"
            name="warningThreshold"
            type="number"
            min="0"
            max="100"
            value={formData.warningThreshold}
            onChange={handleChange}
            required
          />
        </div>

        {budget && (
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
                Active Budget
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
            {creating || updating ? 'Saving...' : budget ? 'Update' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default BudgetFormModal;
