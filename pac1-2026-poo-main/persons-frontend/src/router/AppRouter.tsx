import { Route, Routes } from "react-router"
import { Navbar } from "../presentation/components/layout/Navbar"
import { HomePage, PersonsPage } from "../presentation/pages"

export const AppRouter = () => {
    return (
        <div className="min-h-screen bg-gray-100">
            <Navbar />

            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/persons" element={<PersonsPage />} />
            </Routes>
        </div>
    )
}