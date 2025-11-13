import React, { useState } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import Button from '../../components/common/Button';
import { useCreateTagMutation, CreateTagDto } from './tagApi';
import { useToast } from '../../components/common/ToastContainer';

interface TagFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

const PRESET_COLORS = [
  '#EF4444', '#F97316', '#F59E0B', '#EAB308', '#84CC16',
  '#22C55E', '#10B981', '#14B8A6', '#06B6D4', '#0EA5E9',
  '#3B82F6', '#6366F1', '#8B5CF6', '#A855F7', '#D946EF',
  '#EC4899', '#F43F5E', '#64748B', '#6B7280', '#78716C',
];

const TagFormModal: React.FC<TagFormModalProps> = ({ isOpen, onClose, onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    color: '#3B82F6',
  });

  const { showToast } = useToast();
  const [createTag, { isLoading: creating }] = useCreateTagMutation();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const createDto: CreateTagDto = {
        name: formData.name,
        color: formData.color,
      };
      await createTag(createDto).unwrap();
      onSuccess();
      setFormData({ name: '', color: '#3B82F6' });
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to create tag', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Create Tag" size="md">
      <form onSubmit={handleSubmit}>
        <FormInput
          label="Tag Name"
          name="name"
          value={formData.name}
          onChange={handleChange}
          placeholder="e.g., Business, Personal, Travel"
          required
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

        {/* Actions */}
        <div className="flex justify-end space-x-2 mt-6">
          <Button type="button" variant="secondary" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" disabled={creating}>
            {creating ? 'Creating...' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default TagFormModal;
