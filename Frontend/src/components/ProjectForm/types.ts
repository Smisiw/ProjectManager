import type { Dayjs } from "dayjs";
import type { EmployeeResponse } from "../../types/employee";

export interface ProjectFormState {
    name: string;

    startDate: Dayjs | null;
    endDate: Dayjs | null;

    priority: number;

    customerCompany: string;
    executorCompany: string;

    manager: EmployeeResponse | null;

    selectedEmployees: EmployeeResponse[] | null;

    files: File[];
}