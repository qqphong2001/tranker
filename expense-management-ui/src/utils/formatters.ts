export const formatCurrency = (amount: number, currencySymbol: string = '₫'): string => {
  return `${currencySymbol}${amount.toLocaleString('vi-VN')}`;
};

export const formatDate = (date: string | Date): string => {
  const d = new Date(date);
  return d.toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
  });
};

export const formatDateTime = (date: string | Date): string => {
  const d = new Date(date);
  return d.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
  });
};

export const formatPercentage = (value: number): string => {
  return `${value.toFixed(2)}%`;
};
