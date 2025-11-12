interface StatCardProps {
  title: string;
  value: string;
  icon: string;
  trend?: {
    value: number;
    isPositive: boolean;
  };
  color: string;
}

export default function StatCard({ title, value, icon, trend, color }: StatCardProps) {
  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      <div className="flex items-center justify-between">
        <div className="flex-1">
          <p className="text-sm text-gray-600 dark:text-gray-400 font-medium">{title}</p>
          <p className="text-2xl font-bold text-gray-900 dark:text-white mt-2">{value}</p>
          {trend && (
            <p className={`text-sm mt-2 ${trend.isPositive ? 'text-green-600' : 'text-red-600'}`}>
              {trend.isPositive ? '↑' : '↓'} {Math.abs(trend.value)}%
            </p>
          )}
        </div>
        <div
          className={`w-14 h-14 rounded-full flex items-center justify-center text-2xl ${color}`}
        >
          {icon}
        </div>
      </div>
    </div>
  );
}
