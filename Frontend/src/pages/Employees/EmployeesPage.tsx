import { useEffect, useState } from "react";
import {
    Button,
    Paper,
    Stack,
    TextField,
    Typography
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";

import {createEmployee, deleteEmployee, getEmployees, updateEmployee} from "../../api/employees";
import type { EmployeeResponse } from "../../types/employee";
import { EmployeesGrid } from "./EmployeesGrid";
import { EmployeeDialog } from "./EmployeeDialog";

export function EmployeesPage() {
    const [employees, setEmployees] = useState<EmployeeResponse[]>([]);
    const [search, setSearch] = useState("");

    const [dialogOpen, setDialogOpen] = useState(false);
    const [selectedEmployee, setSelectedEmployee] =
        useState<EmployeeResponse | null>(null);

    async function loadEmployees() {
        const data = await getEmployees(search);

        setEmployees(data);
    }

    async function handleDelete(
        id: string
    ) {
        await deleteEmployee(id);

        await loadEmployees();
    }

    useEffect(() => {
        loadEmployees();
    }, []);

    return (
        <Stack spacing={3}>
            <Stack
                direction="row"
                sx={{
                    justifyContent: "space-between",
                    alignItems: "center"
                }}
            >
                <Typography variant="h4">
                    Employees
                </Typography>

                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => {
                        setSelectedEmployee(null);
                        setDialogOpen(true);
                    }}
                >
                    New Employee
                </Button>
            </Stack>

            <Paper sx={{ p: 2 }}>
                <TextField
                    fullWidth
                    label="Search"
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    onKeyDown={(e) => {
                        if (e.key === "Enter") {
                            loadEmployees();
                        }
                    }}
                />
            </Paper>

            <EmployeesGrid
                rows={employees}
                onEdit={(employee) => {
                    setSelectedEmployee(employee);
                    setDialogOpen(true);
                }}
                onDelete={(employee) => handleDelete(employee.id)}
            />

            <EmployeeDialog
                open={dialogOpen}
                employee={selectedEmployee}
                onCancel={() => setDialogOpen(false)}
                onSubmit={async request => {

                    if (selectedEmployee === null) {

                        await createEmployee(request);

                    } else {

                        await updateEmployee(
                            selectedEmployee.id,
                            request
                        );

                    }

                    setDialogOpen(false);

                    await loadEmployees();

                }}
            />
        </Stack>
    );
}