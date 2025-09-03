import { Routes, Route } from "react-router-dom";
import { Layout } from "@/components/layout/Layout";
import { Dashboard } from "@/pages/Dashboard";
import { BooksPage } from "@/pages/BooksPage";
import { PatronsPage } from "@/pages/PatronsPage";
import { AddBookPage } from "@/pages/AddBookPage";
import { AddPatronPage } from "@/pages/AddPatronPage";
import { EditBookPage } from "@/pages/EditBookPage";
import { EditPatronPage } from "@/pages/EditPatronPage";

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/books" element={<BooksPage />} />
        <Route path="/books/new" element={<AddBookPage />} />
        <Route path="/books/edit/:id" element={<EditBookPage />} />
        <Route path="/patrons" element={<PatronsPage />} />
        <Route path="/patrons/new" element={<AddPatronPage />} />
        <Route path="/patrons/edit/:id" element={<EditPatronPage />} />
      </Routes>
    </Layout>
  );
}

export default App;
