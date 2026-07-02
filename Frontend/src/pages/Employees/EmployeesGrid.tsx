import {
    DataGrid,
    type GridColDef
} from "@mui/x-data-grid";

import {
    IconButton,
    Stack
} from "@mui/material";

import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

import type { EmployeeResponse } from "../../types/employee";

interface Props {
    rows: EmployeeResponse[];

    onEdit(employee: EmployeeResponse): void;

    onDelete(employee: EmployeeResponse): void;
}

export function EmployeesGrid({
                                  rows,
                                  onEdit,
                                  onDelete
                              }: Props) {

    const columns: GridColDef<EmployeeResponse>[] = [
        {
            field: "firstName",
            headerName: "First name",
            flex: 1
        },
        {
            field: "lastName",
            headerName: "Last name",
            flex: 1
        },
        {
            field: "middleName",
            headerName: "Middle name",
            flex: 1
        },
        {
            field: "email",
            headerName: "Email",
            flex: 1.5
        },
        {
            field: "actions",
            headerName: "",
            sortable: false,
            filterable: false,
            width: 110,
            renderCell: ({ row }) => (
                <Stack direction="row">
                    <IconButton
                        onClick={() => onEdit(row)}
                    >
                        <EditIcon />
                    </IconButton>

                    <IconButton
                        onClick={() => onDelete(row)}
                    >
                        <DeleteIcon />
                    </IconButton>
                </Stack>
            )
        }
    ];

    return (
        <DataGrid
            rows={rows}
            columns={columns}
            pageSizeOptions={[10, 25, 50]}
            disableRowSelectionOnClick
        />
    );
}