import React, { useState } from 'react';
import { useGetNotificationsQuery, useMarkAsReadMutation, useMarkAllAsReadMutation, useDeleteNotificationMutation, Notification } from './notificationApi';
import Pagination from '../../components/common/Pagination';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';

const Notifications: React.FC = () => {
  const [page, setPage] = useState(1);
  const [isReadFilter, setIsReadFilter] = useState<boolean | undefined>(undefined);

  const { showToast } = useToast();

  const { data, isLoading, refetch } = useGetNotificationsQuery({
    pageNumber: page,
    pageSize: 10,
    isRead: isReadFilter,
  });

  const [markAsRead] = useMarkAsReadMutation();
  const [markAllAsRead] = useMarkAllAsReadMutation();
  const [deleteNotification] = useDeleteNotificationMutation();

  const handleMarkAsRead = async (notification: Notification) => {
    if (notification.isRead) return;

    try {
      await markAsRead(notification.id).unwrap();
      refetch();
    } catch (error) {
      showToast('Failed to mark notification as read', 'error');
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      await markAllAsRead().unwrap();
      showToast('All notifications marked as read', 'success');
      refetch();
    } catch (error) {
      showToast('Failed to mark all as read', 'error');
    }
  };

  const handleDelete = async (notification: Notification) => {
    if (window.confirm('Are you sure you want to delete this notification?')) {
      try {
        await deleteNotification(notification.id).unwrap();
        showToast('Notification deleted successfully', 'success');
        refetch();
      } catch (error) {
        showToast('Failed to delete notification', 'error');
      }
    }
  };

  const getNotificationIcon = (type: Notification['type']) => {
    switch (type) {
      case 'BudgetExceeded':
        return '🚨';
      case 'BudgetWarning':
        return '⚠️';
      case 'SubscriptionDue':
        return '💳';
      case 'RecurringTransactionCreated':
        return '🔄';
      default:
        return '📢';
    }
  };

  const getNotificationColor = (type: Notification['type']) => {
    switch (type) {
      case 'BudgetExceeded':
        return 'border-red-500 dark:border-red-400';
      case 'BudgetWarning':
        return 'border-yellow-500 dark:border-yellow-400';
      case 'SubscriptionDue':
        return 'border-blue-500 dark:border-blue-400';
      case 'RecurringTransactionCreated':
        return 'border-green-500 dark:border-green-400';
      default:
        return 'border-gray-300 dark:border-gray-600';
    }
  };

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Notifications</h1>
        <Button onClick={handleMarkAllAsRead} variant="secondary">
          Mark All as Read
        </Button>
      </div>

      {/* Filters */}
      <div className="mb-6">
        <div className="flex space-x-2">
          <button
            onClick={() => setIsReadFilter(undefined)}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              isReadFilter === undefined
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            All
          </button>
          <button
            onClick={() => setIsReadFilter(false)}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              isReadFilter === false
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            Unread
          </button>
          <button
            onClick={() => setIsReadFilter(true)}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              isReadFilter === true
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            Read
          </button>
        </div>
      </div>

      {/* Notifications List */}
      {isLoading ? (
        <div className="text-center py-12 text-gray-500 dark:text-gray-400">Loading...</div>
      ) : data?.items.length === 0 ? (
        <div className="text-center py-12 text-gray-500 dark:text-gray-400">No notifications found</div>
      ) : (
        <div className="space-y-3">
          {data?.items.map((notification) => (
            <div
              key={notification.id}
              className={`bg-white dark:bg-gray-800 rounded-lg shadow p-4 border-l-4 ${getNotificationColor(notification.type)} ${
                !notification.isRead ? 'bg-blue-50 dark:bg-gray-700' : ''
              }`}
            >
              <div className="flex items-start justify-between">
                <div className="flex-1 cursor-pointer" onClick={() => handleMarkAsRead(notification)}>
                  <div className="flex items-center space-x-2 mb-1">
                    <span className="text-2xl">{getNotificationIcon(notification.type)}</span>
                    <h3 className="font-semibold text-gray-900 dark:text-white">
                      {notification.title}
                    </h3>
                    {!notification.isRead && (
                      <span className="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200">
                        New
                      </span>
                    )}
                  </div>
                  <p className="text-gray-600 dark:text-gray-400 mb-2">{notification.message}</p>
                  <div className="flex items-center space-x-4 text-xs text-gray-500 dark:text-gray-500">
                    <span>{new Date(notification.createdAt).toLocaleString()}</span>
                    {notification.readAt && (
                      <span>Read at {new Date(notification.readAt).toLocaleString()}</span>
                    )}
                  </div>
                </div>
                <button
                  onClick={() => handleDelete(notification)}
                  className="ml-4 text-red-600 dark:text-red-400 hover:text-red-800 dark:hover:text-red-300"
                  title="Delete notification"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-5 w-5"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                  >
                    <path
                      fillRule="evenodd"
                      d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                      clipRule="evenodd"
                    />
                  </svg>
                </button>
              </div>
            </div>
          ))}
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
    </div>
  );
};

export default Notifications;
