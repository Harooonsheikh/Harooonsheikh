import { formatDate } from '@angular/common';

export class Utilities {
    public now: Date = new Date();
    date: Date;

    addDays(days: number): string {
        this.date = new Date();
        this.date.setDate(this.date.getDate() + days);
        return formatDate(this.date, 'MM/dd/yyyy', 'en-US', '+0530');
    }
}