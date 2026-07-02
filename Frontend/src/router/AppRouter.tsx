import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { Layout } from "../components/Layout/Layout";
import { ProjectsPage } from "../pages/Projects/ProjectsPage";
import { EmployeesPage } from "../pages/Employees/EmployeesPage";
import {ProjectDetailsPage} from "../pages/Projects/ProjectDetailsPage.tsx";

export function AppRouter() {
    return (
        <BrowserRouter>
            <Layout>
                <Routes>
                    <Route
                        path="/"
                        element={<Navigate to="/projects" replace />}
                    />

                    <Route
                        path="/projects"
                        element={<ProjectsPage />}
                    />

                    <Route
                        path="/employees"
                        element={<EmployeesPage />}
                    />

                    <Route
                        path="/projects/:id"
                        element={<ProjectDetailsPage />}
                    />
                </Routes>
            </Layout>
        </BrowserRouter>
    );
}