import { Tabs, Tab } from "@mui/material";
import { Link, useLocation } from "react-router-dom";

export function Navigation() {
    const location = useLocation();

    return (
        <Tabs value={location.pathname} centered>
            <Tab
                value="/projects"
                label="Projects"
                component={Link}
                to="/projects"
            />

            <Tab
                value="/employees"
                label="Employees"
                component={Link}
                to="/employees"
            />
        </Tabs>
    );
}