import {
    Card,
    CardContent,
    Divider,
    List,
    ListItem,
    ListItemText,
    Typography
} from "@mui/material";

import { getEmployeeFullName } from "../../helpers/employee";

import type { ProjectDetailsResponse } from "../../types/project";

interface Props {
    project: ProjectDetailsResponse;
}

export function ProjectTeam({
                                project
                            }: Props) {

    return (
        <Card sx={{ height: "100%" }}>

            <CardContent>

                <Typography
                    variant="h6"
                    gutterBottom
                >
                    Team
                </Typography>

                <Divider sx={{ mb: 2 }} />

                <Typography
                    variant="subtitle2"
                    color="text.secondary"
                >
                    Manager
                </Typography>

                <List disablePadding>

                    <ListItem disableGutters>

                        <ListItemText
                            primary={getEmployeeFullName(project.manager)}
                            secondary={project.manager.email}
                        />

                    </ListItem>

                </List>

                <Divider sx={{ my: 2 }} />

                <Typography
                    variant="subtitle2"
                    color="text.secondary"
                >
                    Team Members
                </Typography>

                <List disablePadding>

                    {project.employees!
                        .map(employee => (

                            <ListItem
                                key={employee.id}
                                disableGutters
                            >

                                <ListItemText
                                    primary={getEmployeeFullName(employee)}
                                    secondary={employee.email}
                                />

                            </ListItem>

                        ))}

                </List>

            </CardContent>

        </Card>
    );
}