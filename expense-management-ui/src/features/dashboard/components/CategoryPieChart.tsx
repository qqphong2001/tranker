import { PieChart, Pie, Cell, ResponsiveContainer, Legend, Tooltip } from 'recharts';
import { CategorySummary } from '../dashboardApi';

interface CategoryPieChartProps {
  data: CategorySummary[];
  title: string;
}

export default function CategoryPieChart({ data, title }: CategoryPieChartProps) {
  const chartData = data.map((cat) => ({
    name: cat.categoryName,
    value: cat.amount,
    color: cat.categoryColor,
  }));

  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6">
      <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">{title}</h3>
      {chartData.length > 0 ? (
        <ResponsiveContainer width="100%" height={300}>
          <PieChart>
            <Pie
              data={chartData}
              cx="50%"
              cy="50%"
              labelLine={false}
              label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
              outerRadius={80}
              fill="#8884d8"
              dataKey="value"
            >
              {chartData.map((entry, index) => (
                <Cell key={`cell-${index}`} fill={entry.color} />
              ))}
            </Pie>
            <Tooltip formatter={(value: number) => `₫${value.toLocaleString()}`} />
            <Legend />
          </PieChart>
        </ResponsiveContainer>
      ) : (
        <div className="h-[300px] flex items-center justify-center">
          <p className="text-gray-500 dark:text-gray-400">No data available</p>
        </div>
      )}
    </div>
  );
}
