export interface Subscription {
  id: string;
  name: string;
  description?: string;
  amount: number;
  startDate: string;
  endDate?: string;
  nextBillingDate: string;
  billingCycle: number;
  isActive: boolean;
  reminderDaysBefore: number;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  currencyId?: string;
  currencyCode?: string;
  currencySymbol?: string;
}

export interface CreateSubscriptionDto {
  name: string;
  description?: string;
  amount: number;
  startDate: string;
  endDate?: string;
  billingCycle: number;
  reminderDaysBefore: number;
  categoryId: string;
  currencyId?: string;
}
