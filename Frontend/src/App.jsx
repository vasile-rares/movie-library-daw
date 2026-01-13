import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import ProtectedRoute from "./guards/ProtectedRoute";
import AdminRoute from "./guards/AdminRoute";
import Login from "./pages/Auth/Login";
import Register from "./pages/Auth/Register";
import Home from "./pages/Home/Home";
import Movies from "./pages/Browse/Movies";
import Shows from "./pages/Browse/Shows";
import TitleDetail from "./pages/Detail/TitleDetail";
import MyList from "./pages/MyList/MyList";
import Settings from "./pages/Settings/Settings";
import Admin from "./pages/Admin/Admin";
import "./App.css";

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <Home />
              </ProtectedRoute>
            }
          />
          <Route
            path="/movies"
            element={
              <ProtectedRoute>
                <Movies />
              </ProtectedRoute>
            }
          />
          <Route
            path="/shows"
            element={
              <ProtectedRoute>
                <Shows />
              </ProtectedRoute>
            }
          />
          <Route
            path="/movie/:id"
            element={
              <ProtectedRoute>
                <TitleDetail titleType="movie" />
              </ProtectedRoute>
            }
          />
          <Route
            path="/show/:id"
            element={
              <ProtectedRoute>
                <TitleDetail titleType="show" />
              </ProtectedRoute>
            }
          />
          <Route
            path="/my-list"
            element={
              <ProtectedRoute>
                <MyList />
              </ProtectedRoute>
            }
          />
          <Route
            path="/settings"
            element={
              <ProtectedRoute>
                <Settings />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin"
            element={
              <AdminRoute>
                <Admin />
              </AdminRoute>
            }
          />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
