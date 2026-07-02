import {
    Card,
    CardContent,
    Divider,
    Table,
    TableBody,
    TableCell,
    TableRow,
    Typography
} from "@mui/material";

import type { ProjectDetailsResponse } from "../../types/project";

interface Props {
    project: ProjectDetailsResponse;
}

export function ProjectInfo({
                                project
                            }: Props) {

    return (
        <Card sx={{ height: "100%" }}>

            <CardContent>

                <Typography
                    variant="h6"
                    gutterBottom
                >
                    General Information
                </Typography>

                <Divider sx={{ mb: 2 }} />

                <Table size="small">

                    <TableBody>

                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>{project.name}</TableCell>
                        </TableRow>

                        <TableRow>
                            <TableCell>Customer</TableCell>
                            <TableCell>{project.customerCompany}</TableCell>
                        </TableRow>

                        <TableRow>
                            <TableCell>Executor</TableCell>
                            <TableCell>{project.executorCompany}</TableCell>
                        </TableRow>

                        <TableRow>
                            <TableCell>Priority</TableCell>
                            <TableCell>{project.priority}</TableCell>
                        </TableRow>

                        <TableRow>
                            <TableCell>Start date</TableCell>
                            <TableCell>{project.startDate}</TableCell>
                        </TableRow>

                        <TableRow>
                            <TableCell>End date</TableCell>
                            <TableCell>{project.endDate}</TableCell>
                        </TableRow>

                    </TableBody>

                </Table>

            </CardContent>

        </Card>
    );
}