export interface Expense {
    id: string;
    amount: number;
    category: string;
    date: Date;
    description?: string;
}