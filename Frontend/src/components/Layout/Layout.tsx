import type { PropsWithChildren } from "react";
import { AppBar, Toolbar, Typography, Container } from "@mui/material";
import { Navigation } from "./Navigation";

export function Layout({ children }: PropsWithChildren) {
    return (
        <>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6">
                        Project Management
                    </Typography>
                </Toolbar>
            </AppBar>

            <Navigation />

            <Container
                maxWidth="xl"
                sx={{
                    mt: 4
                }}
            >
                {children}
            </Container>
        </>
    );
}