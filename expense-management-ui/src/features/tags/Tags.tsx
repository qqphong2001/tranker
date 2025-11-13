import React, { useState } from 'react';
import { useGetTagsQuery, useDeleteTagMutation, Tag } from './tagApi';
import Button from '../../components/common/Button';
import { useToast } from '../../components/common/ToastContainer';
import TagFormModal from './TagFormModal';

const Tags: React.FC = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const { showToast } = useToast();
  const { data: tags, isLoading, refetch } = useGetTagsQuery();
  const [deleteTag] = useDeleteTagMutation();

  const handleDelete = async (tag: Tag) => {
    if (window.confirm(`Are you sure you want to delete "${tag.name}"?`)) {
      try {
        await deleteTag(tag.id).unwrap();
        showToast('Tag deleted successfully', 'success');
        refetch();
      } catch (error: any) {
        showToast(error?.data?.message || 'Failed to delete tag', 'error');
      }
    }
  };

  const handleSuccess = () => {
    showToast('Tag created successfully', 'success');
    setIsModalOpen(false);
    refetch();
  };

  return (
    <div className="p-6">
      <div className="mb-6 flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Tags</h1>
        <Button onClick={() => setIsModalOpen(true)}>Create Tag</Button>
      </div>

      <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-6">
        {isLoading ? (
          <div className="text-center py-12 text-gray-500 dark:text-gray-400">Loading...</div>
        ) : tags?.length === 0 ? (
          <div className="text-center py-12 text-gray-500 dark:text-gray-400">
            No tags found. Create your first tag to get started.
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {tags?.map((tag) => (
              <div
                key={tag.id}
                className="flex items-center justify-between p-4 border border-gray-200 dark:border-gray-700 rounded-lg hover:shadow-md transition-shadow"
              >
                <div className="flex items-center space-x-3 flex-1">
                  <span
                    className="w-4 h-4 rounded-full flex-shrink-0"
                    style={{ backgroundColor: tag.color }}
                  />
                  <span className="font-medium text-gray-900 dark:text-white truncate">
                    {tag.name}
                  </span>
                </div>
                <button
                  onClick={() => handleDelete(tag)}
                  className="ml-2 text-red-600 dark:text-red-400 hover:text-red-800 dark:hover:text-red-300 flex-shrink-0"
                  title="Delete tag"
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
            ))}
          </div>
        )}

        {tags && tags.length > 0 && (
          <div className="mt-6 text-sm text-gray-500 dark:text-gray-400">
            Total tags: {tags.length}
          </div>
        )}
      </div>

      {/* Modal */}
      {isModalOpen && (
        <TagFormModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSuccess={handleSuccess}
        />
      )}
    </div>
  );
};

export default Tags;
