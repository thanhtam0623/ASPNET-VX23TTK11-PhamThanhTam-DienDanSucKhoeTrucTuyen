import React, { useState } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import {
  UserIcon,
  Bars3Icon,
  XMarkIcon,
} from '@heroicons/react/24/outline';
import { useAuth } from '../../context/AuthContext';
import { cn } from '../../utils';

const Header: React.FC = () => {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const navigation = [
    { name: 'Trang chủ', href: '/', current: location.pathname === '/' },
    {
      name: 'Chủ đề Sức khỏe',
      href: '/categories',
      current: location.pathname.startsWith('/categories'),
    },
    {
      name: 'Thảo luận Gần đây',
      href: '/topics',
      current:
        location.pathname.startsWith('/topics') &&
        !location.pathname.includes('/create'),
    },
    {
      name: 'Tư vấn Chuyên gia',
      href: '/experts',
      current: location.pathname.startsWith('/experts'),
    },
  ];

  return (
    <header className="sticky top-0 z-50 border-b border-gray-200 bg-white shadow-sm">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          {/* Logo */}
          <div className="flex items-center">
            <Link to="/" className="flex items-center space-x-3">
              <div className="rounded-xl bg-gradient-to-br from-primary-600 to-secondary-600 p-2.5 text-white shadow-lg">
                <svg
                  className="h-7 w-7"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path d="M19.5 3A2.5 2.5 0 0122 5.5v13a2.5 2.5 0 01-2.5 2.5h-15A2.5 2.5 0 012 18.5v-13A2.5 2.5 0 014.5 3h15zM12 7a1 1 0 00-1 1v8a1 1 0 002 0V8a1 1 0 00-1-1zm3 2a1 1 0 00-1 1v6a1 1 0 002 0v-6a1 1 0 00-1-1zM9 11a1 1 0 00-1 1v4a1 1 0 002 0v-4a1 1 0 00-1-1z" />
                </svg>
              </div>
              <div className="flex flex-col">
                <span className="text-xl font-bold text-gray-900">
                  Sức khỏe
                </span>
                <span className="text-xs font-medium text-gray-500">
                  Cộng đồng
                </span>
              </div>
            </Link>
          </div>

          {/* Desktop Navigation */}
          <nav className="hidden items-center space-x-8 md:flex">
            {navigation.map(item => (
              <Link
                key={item.name}
                to={item.href}
                className={cn(
                  item.current
                    ? 'border-primary-500 text-primary-600'
                    : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700',
                  'inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium transition-colors duration-200'
                )}
              >
                {item.name}
              </Link>
            ))}
          </nav>

          {/* User Menu */}
          <div className="flex items-center space-x-4">
            {isAuthenticated ? (
              <>
                {/* Create Topic Button */}
                <Link
                  to="/topics/create"
                  className="hidden items-center rounded-lg border border-transparent bg-primary-600 px-4 py-2 text-sm font-medium text-white transition-colors duration-200 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:ring-offset-2 sm:inline-flex"
                >
                  Đặt câu hỏi
                </Link>

                {/* User Dropdown */}
                <div className="group relative">
                  <button className="flex items-center space-x-2 rounded-lg p-2 text-sm text-gray-700 transition-all duration-200 hover:bg-gray-100 hover:text-gray-900 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:ring-offset-2">
                    {user?.avatar ? (
                      <img
                        className="h-8 w-8 rounded-full object-cover"
                        src={user.avatar}
                        alt={user.fullName}
                      />
                    ) : (
                      <div className="flex h-8 w-8 items-center justify-center rounded-full bg-primary-100">
                        <UserIcon className="h-5 w-5 text-primary-600" />
                      </div>
                    )}
                    <span className="hidden font-medium md:block">
                      {user?.fullName}
                    </span>
                  </button>

                  {/* Dropdown Menu */}
                  <div className="invisible absolute right-0 z-50 mt-2 w-48 rounded-lg border border-gray-200 bg-white opacity-0 shadow-lg transition-all duration-200 group-hover:visible group-hover:opacity-100">
                    <div className="py-1">
                      <Link
                        to="/profile"
                        className="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 hover:bg-gray-100"
                      >
                        Hồ sơ
                      </Link>
                      <Link
                        to="/profile?tab=topics"
                        className="block px-4 py-2 text-sm text-gray-700 transition-colors duration-200 hover:bg-gray-100"
                      >
                        Chủ đề của tôi
                      </Link>
                      <div className="mt-1 border-t border-gray-100 pt-1">
                        <button
                          onClick={handleLogout}
                          className="w-full px-4 py-2 text-left text-sm text-red-700 transition-colors duration-200 hover:bg-red-50"
                        >
                          Đăng xuất
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </>
            ) : (
              <div className="flex items-center space-x-3">
                <Link
                  to="/login"
                  className="px-3 py-2 text-sm font-medium text-gray-700 transition-colors duration-200 hover:text-gray-900"
                >
                  Đăng nhập
                </Link>
                <Link
                  to="/register"
                  className="rounded-lg bg-primary-600 px-4 py-2 text-sm font-medium text-white"
                >
                  Đăng ký
                </Link>
              </div>
            )}

            {/* Mobile menu button */}
            <button
              onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
              className="inline-flex items-center justify-center rounded-lg p-2 text-gray-400 hover:bg-gray-100 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-primary-500 md:hidden"
            >
              {isMobileMenuOpen ? (
                <XMarkIcon className="h-6 w-6" />
              ) : (
                <Bars3Icon className="h-6 w-6" />
              )}
            </button>
          </div>
        </div>
      </div>

      {/* Mobile menu */}
      {isMobileMenuOpen && (
        <div className="md:hidden">
          <div className="space-y-1 border-t border-gray-200 bg-white px-2 pb-3 pt-2">
            {/* Mobile Navigation */}
            <div className="space-y-1">
              {navigation.map(item => (
                <Link
                  key={item.name}
                  to={item.href}
                  onClick={() => setIsMobileMenuOpen(false)}
                  className={cn(
                    item.current
                      ? 'border-primary-500 bg-primary-50 text-primary-700'
                      : 'border-transparent text-gray-600 hover:border-gray-300 hover:bg-gray-50 hover:text-gray-800',
                    'block border-l-4 py-2 pl-3 pr-4 text-base font-medium transition-colors duration-200'
                  )}
                >
                  {item.name}
                </Link>
              ))}
            </div>

            {/* Mobile Auth Links */}
            {isAuthenticated ? (
              <div className="border-t border-gray-200 pb-3 pt-4">
                <div className="space-y-1 px-3 py-2">
                  <Link
                    to="/topics/create"
                    onClick={() => setIsMobileMenuOpen(false)}
                    className="block rounded-lg bg-primary-600 px-3 py-2 text-base font-medium text-white"
                  >
                    Đặt câu hỏi
                  </Link>
                  <Link
                    to="/profile"
                    onClick={() => setIsMobileMenuOpen(false)}
                    className="block px-3 py-2 text-base font-medium text-gray-600 hover:text-gray-900"
                  >
                    Hồ sơ
                  </Link>
                  <button
                    onClick={() => {
                      handleLogout();
                      setIsMobileMenuOpen(false);
                    }}
                    className="block w-full px-3 py-2 text-left text-base font-medium text-red-600 hover:text-red-900"
                  >
                    Đăng xuất
                  </button>
                </div>
              </div>
            ) : (
              <div className="border-t border-gray-200 pb-3 pt-4">
                <div className="space-y-2 px-3">
                  <Link
                    to="/login"
                    onClick={() => setIsMobileMenuOpen(false)}
                    className="block px-3 py-2 text-base font-medium text-gray-600 hover:text-gray-900"
                  >
                    Đăng nhập
                  </Link>
                  <Link
                    to="/register"
                    onClick={() => setIsMobileMenuOpen(false)}
                    className="block rounded-lg bg-primary-600 px-3 py-2 text-base font-medium text-white"
                  >
                    Đăng ký
                  </Link>
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </header>
  );
};

export default Header;
