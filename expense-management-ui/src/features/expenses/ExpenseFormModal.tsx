import React, { useState, useEffect } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import FormSelect from '../../components/common/FormSelect';
import Button from '../../components/common/Button';
import { useCreateExpenseMutation, useUpdateExpenseMutation, Expense, CreateExpenseDto, UpdateExpenseDto } from './expenseApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import { useGetTagsQuery } from '../tags/tagApi';
import { useToast } from '../../components/common/ToastContainer';

interface ExpenseFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  expense?: Expense | null;
  onSuccess: () => void;
}

const ExpenseFormModal: React.FC<ExpenseFormModalProps> = ({ isOpen, onClose, expense, onSuccess }) => {
  const [formData, setFormData] = useState({
    amount: '',
    description: '',
    date: new Date().toISOString().split('T')[0],
    notes: '',
    categoryId: '',
    tagIds: [] as string[],
  });

  const { showToast } = useToast();
  const { data: categories, isLoading: categoriesLoading } = useGetCategoriesQuery({ transactionType: 'Expense' });
  const { data: tags } = useGetTagsQuery();
  const [createExpense, { isLoading: creating }] = useCreateExpenseMutation();
  const [updateExpense, { isLoading: updating }] = useUpdateExpenseMutation();

  useEffect(() => {
    if (expense) {
      setFormData({
        amount: expense.amount.toString(),
        description: expense.description || '',
        date: expense.date.split('T')[0],
        notes: expense.notes || '',
        categoryId: expense.categoryId,
        tagIds: expense.tags?.map(t => t.id) || [],
      });
    }
  }, [expense]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleTagToggle = (tagId: string) => {
    setFormData((prev) => ({
      ...prev,
      tagIds: prev.tagIds.includes(tagId)
        ? prev.tagIds.filter((id) => id !== tagId)
        : [...prev.tagIds, tagId],
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.categoryId) {
      showToast('Please select a category', 'error');
      return;
    }

    try {
      if (expense) {
        const updateDto: UpdateExpenseDto = {
          id: expense.id,
          amount: parseFloat(formData.amount),
          description: formData.description,
          date: formData.date,
          notes: formData.notes,
          categoryId: formData.categoryId,
          tagIds: formData.tagIds,
        };
        await updateExpense(updateDto).unwrap();
      } else {
        const createDto: CreateExpenseDto = {
          amount: parseFloat(formData.amount),
          description: formData.description,
          date: formData.date,
          notes: formData.notes,
          categoryId: formData.categoryId,
          tagIds: formData.tagIds,
        };
        await createExpense(createDto).unwrap();
      }
      onSuccess();
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to save expense', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={expense ? 'Edit Expense' : 'Add Expense'} size="lg">
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
            placeholder="e.g., Groceries"
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

        {/* Tags */}
        {tags && tags.length > 0 && (
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Tags</label>
            <div className="flex flex-wrap gap-2">
              {tags.map((tag) => (
                <button
                  key={tag.id}
                  type="button"
                  onClick={() => handleTagToggle(tag.id)}
                  className={`px-3 py-1 rounded-full text-sm transition-all ${
                    formData.tagIds.includes(tag.id)
                      ? 'bg-blue-600 text-white'
                      : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300'
                  }`}
                >
                  {tag.name}
                </button>
              ))}
            </div>
          </div>
        )}

        {/* Actions */}
        <div className="flex justify-end space-x-2 mt-6">
          <Button type="button" variant="secondary" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" disabled={creating || updating}>
            {creating || updating ? 'Saving...' : expense ? 'Update' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default ExpenseFormModal;
