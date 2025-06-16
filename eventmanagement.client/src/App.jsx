import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import './App.css';
import Events from './components/Events';
import SalesSummary from './components/salesSummary';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import { ToastContainer } from 'react-toastify';
function App() {

    return (
        <>
            <Router>
                <Navbar />
                <main className="container my-5">
                    <Routes>
                        <Route path="/" element={<Events />} />
                        <Route path="/sales-summary" element={<SalesSummary />} />
                    </Routes>
                </main>
            </Router>
            <ToastContainer
                position="top-left"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="colored"
                transition: Bounce
            />
            <Footer />
        </>
    );

}

export default App;