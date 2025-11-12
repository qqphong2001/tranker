export interface Expense {
  id: string;
  amount: number;
  description?: string;
  date: string;
  notes?: string;
  imageUrl?: string;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  categoryIcon?: string;
  currencyId?: string;
  currencyCode?: string;
  currencySymbol?: string;
  tags: Tag[];
  createdAt: string;
}

export interface Tag {
  id: string;
  name: string;
  color?: string;
}

export interface CreateExpenseDto {
  amount: number;
  description?: string;
  date: string;
  notes?: string;
  imageUrl?: string;
  categoryId: string;
  currencyId?: string;
  tagIds: string[];
}

export interface UpdateExpenseDto extends CreateExpenseDto {
  id: string;
}
