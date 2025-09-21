import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/Layout/Layout';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import TopicDetailPage from './pages/TopicDetailPage';
import TopicsPage from './pages/TopicsPage';
import CategoryPage from './pages/CategoryPage';
import CategoriesPage from './pages/CategoriesPage';
import CreateTopicPage from './pages/CreateTopicPage';
import ProfilePage from './pages/ProfilePage';
import UserProfilePage from './pages/UserProfilePage';
import SearchPage from './pages/SearchPage';
import EmergencyPage from './pages/EmergencyPage';
import ExpertsPage from './pages/ExpertsPage';
import NotFoundPage from './pages/NotFoundPage';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            {/* Public routes */}
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/search" element={<SearchPage />} />
            <Route path="/emergency" element={<EmergencyPage />} />

            {/* Topic routes */}
            <Route path="/topics" element={<TopicsPage />} />
            <Route path="/topics/:slug" element={<TopicDetailPage />} />
            <Route path="/topics/create" element={<CreateTopicPage />} />

            {/* Category routes */}
            <Route path="/categories" element={<CategoriesPage />} />
            <Route path="/categories/:slug" element={<CategoryPage />} />

            {/* Expert routes */}
            <Route path="/experts" element={<ExpertsPage />} />

            {/* Profile routes */}
            <Route path="/profile" element={<ProfilePage />} />
            <Route path="/users/:username" element={<UserProfilePage />} />

            {/* 404 */}
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App;
