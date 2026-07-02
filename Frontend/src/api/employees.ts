import { api } from "./api";
import type {
    CreateEmployeeRequest,
    EmployeeResponse,
    UpdateEmployeeRequest,
} from "../types/employee";

export async function getEmployees(
    search?: string,
): Promise<EmployeeResponse[]> {
    const response = await api.get<EmployeeResponse[]>("/employees", {
        params: {
            search: search || undefined,
        },
    });

    return response.data;
}

export async function createEmployee(
    request: CreateEmployeeRequest,
): Promise<EmployeeResponse> {
    const response = await api.post<EmployeeResponse>(
        "/employees",
        request,
    );

    return response.data;
}

export async function updateEmployee(
    id: string,
    request: UpdateEmployeeRequest,
): Promise<EmployeeResponse> {
    const response = await api.put<EmployeeResponse>(
        `/employees/${id}`,
        request,
    );

    return response.data;
}

export async function deleteEmployee(id: string): Promise<void> {
    await api.delete(`/employees/${id}`);
}