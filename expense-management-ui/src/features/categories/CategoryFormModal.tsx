import React, { useState, useEffect } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import FormSelect from '../../components/common/FormSelect';
import Button from '../../components/common/Button';
import { useCreateCategoryMutation, useUpdateCategoryMutation, Category, CreateCategoryDto, UpdateCategoryDto } from './categoryApi';
import { useToast } from '../../components/common/ToastContainer';

interface CategoryFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  category?: Category | null;
  onSuccess: () => void;
}

const PRESET_COLORS = [
  '#EF4444', '#F97316', '#F59E0B', '#EAB308', '#84CC16',
  '#22C55E', '#10B981', '#14B8A6', '#06B6D4', '#0EA5E9',
  '#3B82F6', '#6366F1', '#8B5CF6', '#A855F7', '#D946EF',
  '#EC4899', '#F43F5E', '#64748B', '#6B7280', '#78716C',
];

const PRESET_ICONS = [
  '🏠', '🍔', '🚗', '🎬', '🏥', '📚', '💰', '🎮', '✈️', '👕',
  '⚡', '📱', '🎵', '🏋️', '🍷', '🎨', '🔧', '💼', '🎓', '🛒',
];

const CategoryFormModal: React.FC<CategoryFormModalProps> = ({ isOpen, onClose, category, onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    icon: '',
    color: '#3B82F6',
    transactionType: 'Expense' as 'Income' | 'Expense',
  });

  const { showToast } = useToast();
  const [createCategory, { isLoading: creating }] = useCreateCategoryMutation();
  const [updateCategory, { isLoading: updating }] = useUpdateCategoryMutation();

  useEffect(() => {
    if (category) {
      setFormData({
        name: category.name,
        description: category.description || '',
        icon: category.icon || '',
        color: category.color || '#3B82F6',
        transactionType: category.transactionType,
      });
    } else {
      setFormData({
        name: '',
        description: '',
        icon: '',
        color: '#3B82F6',
        transactionType: 'Expense',
      });
    }
  }, [category, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (category) {
        const updateDto: UpdateCategoryDto = {
          id: category.id,
          name: formData.name,
          description: formData.description,
          icon: formData.icon,
          color: formData.color,
        };
        await updateCategory(updateDto).unwrap();
      } else {
        const createDto: CreateCategoryDto = {
          name: formData.name,
          description: formData.description,
          icon: formData.icon,
          color: formData.color,
          transactionType: formData.transactionType,
        };
        await createCategory(createDto).unwrap();
      }
      onSuccess();
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to save category', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={category ? 'Edit Category' : 'Create Category'} size="lg">
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormInput
            label="Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="e.g., Food & Dining"
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
            disabled={!!category}
          />
        </div>

        <FormInput
          label="Description"
          name="description"
          value={formData.description}
          onChange={handleChange}
          placeholder="Category description..."
        />

        {/* Color Picker */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
            Color
          </label>
          <div className="flex items-center space-x-2 mb-2">
            <input
              type="color"
              name="color"
              value={formData.color}
              onChange={handleChange}
              className="h-10 w-20 rounded border border-gray-300 dark:border-gray-600 cursor-pointer"
            />
            <span className="text-sm text-gray-600 dark:text-gray-400">{formData.color}</span>
          </div>
          <div className="grid grid-cols-10 gap-2">
            {PRESET_COLORS.map((color) => (
              <button
                key={color}
                type="button"
                onClick={() => setFormData((prev) => ({ ...prev, color }))}
                className={`h-8 w-8 rounded border-2 transition-all ${
                  formData.color === color
                    ? 'border-gray-900 dark:border-white scale-110'
                    : 'border-transparent hover:scale-105'
                }`}
                style={{ backgroundColor: color }}
                title={color}
              />
            ))}
          </div>
        </div>

        {/* Icon Picker */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
            Icon (Optional)
          </label>
          <FormInput
            name="icon"
            value={formData.icon}
            onChange={handleChange}
            placeholder="Enter emoji or leave empty"
            className="mb-2"
          />
          <div className="grid grid-cols-10 gap-2">
            {PRESET_ICONS.map((icon) => (
              <button
                key={icon}
                type="button"
                onClick={() => setFormData((prev) => ({ ...prev, icon }))}
                className={`h-10 w-10 text-2xl rounded border-2 transition-all ${
                  formData.icon === icon
                    ? 'border-blue-500 dark:border-blue-400 bg-blue-50 dark:bg-blue-900'
                    : 'border-gray-300 dark:border-gray-600 hover:border-blue-300 dark:hover:border-blue-500'
                }`}
                title={icon}
              >
                {icon}
              </button>
            ))}
          </div>
        </div>

        {/* Actions */}
        <div className="flex justify-end space-x-2 mt-6">
          <Button type="button" variant="secondary" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" disabled={creating || updating}>
            {creating || updating ? 'Saving...' : category ? 'Update' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CategoryFormModal;
