import {
    DataGrid,
    type GridColDef,
    type GridSortModel
} from "@mui/x-data-grid";

import {
    IconButton,
    Stack
} from "@mui/material";

import DeleteIcon from "@mui/icons-material/Delete";

import { useNavigate } from "react-router-dom";

import type { ProjectResponse } from "../../types/project";

interface Props {
    rows: ProjectResponse[];

    onDelete(id: string): void;

    sortModel: GridSortModel;

    setSortModel(model: GridSortModel): void;
}

export function ProjectsGrid({
                                 rows,
                                 onDelete,
                                 sortModel,
                                 setSortModel
                             }: Props) {

    const navigate = useNavigate();

    const columns: GridColDef<ProjectResponse>[] = [
        {
            field: "name",
            headerName: "Name",
            flex: 1.5
        },
        {
            field: "customerCompany",
            headerName: "Customer",
            flex: 1.2
        },
        {
            field: "executorCompany",
            headerName: "Executor",
            flex: 1.2
        },
        {
            field: "startDate",
            headerName: "Start",
            flex: 1
        },
        {
            field: "endDate",
            headerName: "End",
            flex: 1
        },
        {
            field: "priority",
            headerName: "Priority",
            width: 100
        },
        {
            field: "manager",
            headerName: "Manager",
            flex: 1.3,
            sortable: false,
            valueGetter: (_, row) => row.manager.fullName
        },
        {
            field: "actions",
            headerName: "",
            sortable: false,
            filterable: false,
            width: 80,
            align: "center",
            renderCell: ({ row }) => (
                <Stack
                    direction="row"
                    spacing={1}
                >
                    <IconButton
                        color="error"
                        onClick={(e) => {
                            e.stopPropagation();
                            onDelete(row.id);
                        }}
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
            disableRowSelectionOnClick
            sortModel={sortModel}
            onSortModelChange={setSortModel}
            onRowClick={(params) =>
                navigate(`/projects/${params.row.id}`)
            }
            pageSizeOptions={[10, 25, 50]}
            initialState={{
                pagination: {
                    paginationModel: {
                        pageSize: 10,
                        page: 0
                    }
                }
            }}
        />
    );
}