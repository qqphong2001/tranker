export enum BudgetPeriod {
  Monthly = 1,
  Quarterly = 2,
  Yearly = 3,
}

export interface Budget {
  id: string;
  name: string;
  amount: number;
  spentAmount: number;
  remainingAmount: number;
  percentage: number;
  period: BudgetPeriod;
  startDate: string;
  endDate: string;
  warningThreshold: number;
  isActive: boolean;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  isExceeded: boolean;
  isWarning: boolean;
}

export interface CreateBudgetDto {
  name: string;
  amount: number;
  period: BudgetPeriod;
  startDate: string;
  warningThreshold: number;
  categoryId: string;
}
