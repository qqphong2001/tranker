import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { MonthlyTrend } from '../dashboardApi';

interface IncomeExpenseChartProps {
  data: MonthlyTrend[];
}

export default function IncomeExpenseChart({ data }: IncomeExpenseChartProps) {
  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 col-span-2">
      <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Income vs Expenses (6 Months)</h3>
      {data.length > 0 ? (
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={data}>
            <CartesianGrid strokeDasharray="3 3" stroke="#374151" />
            <XAxis dataKey="monthName" stroke="#9CA3AF" />
            <YAxis stroke="#9CA3AF" />
            <Tooltip
              contentStyle={{
                backgroundColor: '#1F2937',
                border: 'none',
                borderRadius: '8px',
                color: '#F3F4F6',
              }}
              formatter={(value: number) => `₫${value.toLocaleString()}`}
            />
            <Legend />
            <Line
              type="monotone"
              dataKey="income"
              stroke="#10B981"
              strokeWidth={2}
              name="Income"
              dot={{ fill: '#10B981' }}
            />
            <Line
              type="monotone"
              dataKey="expenses"
              stroke="#EF4444"
              strokeWidth={2}
              name="Expenses"
              dot={{ fill: '#EF4444' }}
            />
          </LineChart>
        </ResponsiveContainer>
      ) : (
        <div className="h-[300px] flex items-center justify-center">
          <p className="text-gray-500 dark:text-gray-400">No data available</p>
        </div>
      )}
    </div>
  );
}
