import React, { useState, useEffect } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import FormSelect from '../../components/common/FormSelect';
import Button from '../../components/common/Button';
import { useCreateIncomeMutation, useUpdateIncomeMutation, Income, CreateIncomeDto, UpdateIncomeDto } from './incomeApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import { useToast } from '../../components/common/ToastContainer';

interface IncomeFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  income?: Income | null;
  onSuccess: () => void;
}

const IncomeFormModal: React.FC<IncomeFormModalProps> = ({ isOpen, onClose, income, onSuccess }) => {
  const [formData, setFormData] = useState({
    amount: '',
    description: '',
    date: new Date().toISOString().split('T')[0],
    notes: '',
    source: '',
    categoryId: '',
  });

  const { showToast } = useToast();
  const { data: categories, isLoading: categoriesLoading } = useGetCategoriesQuery({ transactionType: 'Income' });
  const [createIncome, { isLoading: creating }] = useCreateIncomeMutation();
  const [updateIncome, { isLoading: updating }] = useUpdateIncomeMutation();

  useEffect(() => {
    if (income) {
      setFormData({
        amount: income.amount.toString(),
        description: income.description || '',
        date: income.date.split('T')[0],
        notes: income.notes || '',
        source: income.source || '',
        categoryId: income.categoryId,
      });
    }
  }, [income]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.categoryId) {
      showToast('Please select a category', 'error');
      return;
    }

    try {
      if (income) {
        const updateDto: UpdateIncomeDto = {
          id: income.id,
          amount: parseFloat(formData.amount),
          description: formData.description,
          date: formData.date,
          notes: formData.notes,
          source: formData.source,
          categoryId: formData.categoryId,
        };
        await updateIncome(updateDto).unwrap();
      } else {
        const createDto: CreateIncomeDto = {
          amount: parseFloat(formData.amount),
          description: formData.description,
          date: formData.date,
          notes: formData.notes,
          source: formData.source,
          categoryId: formData.categoryId,
        };
        await createIncome(createDto).unwrap();
      }
      onSuccess();
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to save income', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={income ? 'Edit Income' : 'Add Income'} size="lg">
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormInput
            label="Amount"
            name="amount"
            type="number"
            step="0.01"
            value={formData.amount}
            onChange={handleChange}
            required
          />

          <FormInput
            label="Date"
            name="date"
            type="date"
            value={formData.date}
            onChange={handleChange}
            required
          />

          <FormInput
            label="Description"
            name="description"
            value={formData.description}
            onChange={handleChange}
            placeholder="e.g., Salary"
          />

          <FormInput
            label="Source"
            name="source"
            value={formData.source}
            onChange={handleChange}
            placeholder="e.g., Company Name"
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
        </div>

        <FormInput
          label="Notes"
          name="notes"
          value={formData.notes}
          onChange={handleChange}
          placeholder="Additional notes..."
        />

        {/* Actions */}
        <div className="flex justify-end space-x-2 mt-6">
          <Button type="button" variant="secondary" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" disabled={creating || updating}>
            {creating || updating ? 'Saving...' : income ? 'Update' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default IncomeFormModal;
