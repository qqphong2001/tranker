import React, { useState, useEffect } from 'react';
import Modal from '../../components/common/Modal';
import FormInput from '../../components/common/FormInput';
import FormSelect from '../../components/common/FormSelect';
import Button from '../../components/common/Button';
import { useCreateSubscriptionMutation, useUpdateSubscriptionMutation, Subscription, CreateSubscriptionDto, UpdateSubscriptionDto } from './subscriptionApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import { useToast } from '../../components/common/ToastContainer';

interface SubscriptionFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  subscription?: Subscription | null;
  onSuccess: () => void;
}

const SubscriptionFormModal: React.FC<SubscriptionFormModalProps> = ({ isOpen, onClose, subscription, onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    amount: '',
    startDate: new Date().toISOString().split('T')[0],
    endDate: '',
    billingCycle: '30',
    reminderDaysBefore: '3',
    categoryId: '',
    isActive: true,
  });

  const { showToast } = useToast();
  const { data: categories, isLoading: categoriesLoading } = useGetCategoriesQuery({ transactionType: 'Expense' });
  const [createSubscription, { isLoading: creating }] = useCreateSubscriptionMutation();
  const [updateSubscription, { isLoading: updating }] = useUpdateSubscriptionMutation();

  useEffect(() => {
    if (subscription) {
      setFormData({
        name: subscription.name,
        description: subscription.description || '',
        amount: subscription.amount.toString(),
        startDate: subscription.startDate.split('T')[0],
        endDate: subscription.endDate ? subscription.endDate.split('T')[0] : '',
        billingCycle: subscription.billingCycle.toString(),
        reminderDaysBefore: subscription.reminderDaysBefore.toString(),
        categoryId: subscription.categoryId,
        isActive: subscription.isActive,
      });
    } else {
      setFormData({
        name: '',
        description: '',
        amount: '',
        startDate: new Date().toISOString().split('T')[0],
        endDate: '',
        billingCycle: '30',
        reminderDaysBefore: '3',
        categoryId: '',
        isActive: true,
      });
    }
  }, [subscription, isOpen]);

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
      if (subscription) {
        const updateDto: UpdateSubscriptionDto = {
          id: subscription.id,
          name: formData.name,
          description: formData.description,
          amount: parseFloat(formData.amount),
          billingCycle: parseInt(formData.billingCycle),
          reminderDaysBefore: parseInt(formData.reminderDaysBefore),
          isActive: formData.isActive,
          categoryId: formData.categoryId,
        };
        await updateSubscription(updateDto).unwrap();
      } else {
        const createDto: CreateSubscriptionDto = {
          name: formData.name,
          description: formData.description,
          amount: parseFloat(formData.amount),
          startDate: formData.startDate,
          endDate: formData.endDate || undefined,
          billingCycle: parseInt(formData.billingCycle),
          reminderDaysBefore: parseInt(formData.reminderDaysBefore),
          categoryId: formData.categoryId,
        };
        await createSubscription(createDto).unwrap();
      }
      onSuccess();
    } catch (error: any) {
      showToast(error?.data?.message || 'Failed to save subscription', 'error');
    }
  };

  const getBillingCycleLabel = () => {
    const cycle = parseInt(formData.billingCycle);
    if (cycle === 7) return 'Weekly';
    if (cycle === 30) return 'Monthly';
    if (cycle === 90) return 'Quarterly';
    if (cycle === 365) return 'Yearly';
    return `Every ${cycle} days`;
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={subscription ? 'Edit Subscription' : 'Create Subscription'} size="lg">
      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormInput
            label="Subscription Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="e.g., Netflix Premium"
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
            label="Billing Cycle"
            name="billingCycle"
            value={formData.billingCycle}
            onChange={handleChange}
            options={[
              { value: '7', label: 'Weekly (7 days)' },
              { value: '30', label: 'Monthly (30 days)' },
              { value: '90', label: 'Quarterly (90 days)' },
              { value: '365', label: 'Yearly (365 days)' },
            ]}
            required
          />

          <FormInput
            label="Reminder (days before)"
            name="reminderDaysBefore"
            type="number"
            min="0"
            value={formData.reminderDaysBefore}
            onChange={handleChange}
            required
          />

          {!subscription && (
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
            placeholder="Subscription details..."
          />
        </div>

        {subscription && (
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
                Active Subscription
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
            {creating || updating ? 'Saving...' : subscription ? 'Update' : 'Create'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default SubscriptionFormModal;
