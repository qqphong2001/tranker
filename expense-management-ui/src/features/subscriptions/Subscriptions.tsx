import React, { useState } from 'react';
import { useGetSubscriptionsQuery, useDeleteSubscriptionMutation, Subscription } from './subscriptionApi';
import { useGetCategoriesQuery } from '../categories/categoryApi';
import Pagination from '../../components/common/Pagination';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import SubscriptionFormModal from './SubscriptionFormModal';

const Subscriptions: React.FC = () => {
  const [page, setPage] = useState(1);
  const [categoryFilter, setCategoryFilter] = useState('');
  const [activeFilter, setActiveFilter] = useState<boolean | undefined>(undefined);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedSubscription, setSelectedSubscription] = useState<Subscription | null>(null);

  const { showToast } = useToast();

  const { data, isLoading, refetch } = useGetSubscriptionsQuery({
    pageNumber: page,
    pageSize: 10,
    categoryId: categoryFilter || undefined,
    isActive: activeFilter,
  });

  const { data: categories } = useGetCategoriesQuery({ transactionType: 'Expense' });
  const [deleteSubscription] = useDeleteSubscriptionMutation();

  const handleEdit = (subscription: Subscription) => {
    setSelectedSubscription(subscription);
    setIsModalOpen(true);
  };

  const handleDelete = async (subscription: Subscription) => {
    if (window.confirm(`Are you sure you want to delete "${subscription.name}"?`)) {
      try {
        await deleteSubscription(subscription.id).unwrap();
        showToast('Subscription deleted successfully', 'success');
        refetch();
      } catch (error) {
        showToast('Failed to delete subscription', 'error');
      }
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedSubscription(null);
  };

  const handleSuccess = () => {
    showToast(selectedSubscription ? 'Subscription updated successfully' : 'Subscription created successfully', 'success');
    handleModalClose();
    refetch();
  };

  const getBillingCycleLabel = (days: number) => {
    if (days === 7) return 'Weekly';
    if (days === 30) return 'Monthly';
    if (days === 90) return 'Quarterly';
    if (days === 365) return 'Yearly';
    return `Every ${days} days`;
  };

  const getDaysUntilBilling = (nextBillingDate: string) => {
    const next = new Date(nextBillingDate);
    const today = new Date();
    const diff = Math.ceil((next.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    return diff;
  };

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Subscriptions</h1>
        <Button onClick={() => setIsModalOpen(true)}>Add Subscription</Button>
      </div>

      {/* Filters */}
      <div className="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
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

      {/* Subscription Cards */}
      {isLoading ? (
        <div className="text-center py-12 text-gray-500 dark:text-gray-400">Loading...</div>
      ) : data?.items.length === 0 ? (
        <div className="text-center py-12 text-gray-500 dark:text-gray-400">No subscriptions found</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {data?.items.map((subscription) => {
            const daysUntil = getDaysUntilBilling(subscription.nextBillingDate);
            return (
              <div
                key={subscription.id}
                className={`bg-white dark:bg-gray-800 rounded-lg shadow p-6 ${
                  !subscription.isActive ? 'opacity-60' : ''
                }`}
              >
                <div className="flex justify-between items-start mb-4">
                  <div className="flex-1">
                    <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                      {subscription.name}
                    </h3>
                    <div className="flex items-center mt-1 space-x-2">
                      <span
                        className="w-3 h-3 rounded-full"
                        style={{ backgroundColor: subscription.categoryColor }}
                      />
                      <span className="text-sm text-gray-600 dark:text-gray-400">
                        {subscription.categoryName}
                      </span>
                    </div>
                  </div>
                  <span
                    className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                      subscription.isActive
                        ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'
                        : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'
                    }`}
                  >
                    {subscription.isActive ? 'Active' : 'Inactive'}
                  </span>
                </div>

                {subscription.description && (
                  <p className="text-sm text-gray-600 dark:text-gray-400 mb-4">
                    {subscription.description}
                  </p>
                )}

                <div className="space-y-2 mb-4">
                  <div className="flex justify-between items-center">
                    <span className="text-sm text-gray-600 dark:text-gray-400">Amount</span>
                    <span className="text-lg font-bold text-gray-900 dark:text-white">
                      {subscription.currencySymbol || '$'}{subscription.amount.toFixed(2)}
                    </span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-sm text-gray-600 dark:text-gray-400">Billing Cycle</span>
                    <span className="text-sm text-gray-900 dark:text-white">
                      {getBillingCycleLabel(subscription.billingCycle)}
                    </span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-sm text-gray-600 dark:text-gray-400">Next Billing</span>
                    <span className="text-sm text-gray-900 dark:text-white">
                      {new Date(subscription.nextBillingDate).toLocaleDateString()}
                    </span>
                  </div>
                  {subscription.isActive && (
                    <div className="pt-2">
                      <span
                        className={`text-xs font-medium ${
                          daysUntil <= subscription.reminderDaysBefore
                            ? 'text-orange-600 dark:text-orange-400'
                            : 'text-gray-600 dark:text-gray-400'
                        }`}
                      >
                        {daysUntil <= 0
                          ? 'Due today!'
                          : daysUntil === 1
                          ? '1 day until billing'
                          : `${daysUntil} days until billing`}
                      </span>
                    </div>
                  )}
                </div>

                <div className="flex space-x-2 pt-4 border-t border-gray-200 dark:border-gray-700">
                  <button
                    onClick={() => handleEdit(subscription)}
                    className="flex-1 text-sm text-blue-600 dark:text-blue-400 hover:text-blue-800 dark:hover:text-blue-300 font-medium"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(subscription)}
                    className="flex-1 text-sm text-red-600 dark:text-red-400 hover:text-red-800 dark:hover:text-red-300 font-medium"
                  >
                    Delete
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {data && data.totalPages > 1 && (
        <div className="mt-6">
          <Pagination
            currentPage={data.pageNumber}
            totalPages={data.totalPages}
            hasNextPage={data.hasNextPage}
            hasPreviousPage={data.hasPreviousPage}
            onPageChange={setPage}
          />
        </div>
      )}

      {/* Modal */}
      {isModalOpen && (
        <SubscriptionFormModal
          isOpen={isModalOpen}
          onClose={handleModalClose}
          subscription={selectedSubscription}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default Subscriptions;
