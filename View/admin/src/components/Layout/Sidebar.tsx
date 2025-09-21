import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  HomeIcon,
  UsersIcon,
  FolderIcon,
  DocumentTextIcon,
  AcademicCapIcon,
  ChartBarIcon,
  ShieldCheckIcon,
} from '@heroicons/react/24/outline';
import { cn } from '../../utils';

interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onClose }) => {
  const location = useLocation();

  const navigation = [
    {
      name: 'Bảng điều khiển',
      href: '/',
      icon: HomeIcon,
      current: location.pathname === '/',
    },
    {
      name: 'Người dùng',
      href: '/users',
      icon: UsersIcon,
      current: location.pathname.startsWith('/users'),
    },
    {
      name: 'Danh mục',
      href: '/categories',
      icon: FolderIcon,
      current: location.pathname.startsWith('/categories'),
    },
    {
      name: 'Chủ đề',
      href: '/topics',
      icon: DocumentTextIcon,
      current: location.pathname.startsWith('/topics'),
    },
    {
      name: 'Chuyên gia',
      href: '/experts',
      icon: AcademicCapIcon,
      current: location.pathname.startsWith('/experts'),
    },
    {
      name: 'Báo cáo',
      href: '/reports',
      icon: ChartBarIcon,
      current: location.pathname.startsWith('/reports'),
    },
  ];

  return (
    <>
      {/* Mobile overlay */}
      {isOpen && (
        <div
          className="fixed inset-0 z-40 bg-gray-600 bg-opacity-75 lg:hidden"
          onClick={onClose}
        />
      )}

      {/* Sidebar */}
      <div
        className={cn(
          'fixed inset-y-0 left-0 z-50 w-64 transform bg-white shadow-lg transition-transform duration-300 ease-in-out lg:static lg:translate-x-0',
          isOpen ? 'translate-x-0' : '-translate-x-full'
        )}
      >
        <div className="flex h-full flex-col">
          {/* Logo */}
          <div className="flex h-16 flex-shrink-0 items-center border-b border-gray-200 px-6">
            <div className="flex items-center space-x-3">
              <div className="rounded-lg bg-gradient-to-br from-primary-600 to-secondary-600 p-2 text-white shadow-lg">
                <ShieldCheckIcon className="h-6 w-6" />
              </div>
              <div className="flex flex-col">
                <span className="text-lg font-bold text-gray-900">
                  Y tế
                </span>
                <span className="text-xs font-medium text-gray-500">
                  Quản trị
                </span>
              </div>
            </div>
          </div>

          {/* Navigation */}
          <nav className="flex-1 space-y-1 px-3 py-4">
            {navigation.map(item => (
              <Link
                key={item.name}
                to={item.href}
                onClick={() => onClose()}
                className={cn(
                  item.current
                    ? 'bg-primary-100 text-primary-900'
                    : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900',
                  'group flex items-center rounded-lg px-3 py-2 text-sm font-medium transition-colors duration-200'
                )}
              >
                <item.icon
                  className={cn(
                    item.current
                      ? 'text-primary-600'
                      : 'text-gray-400 group-hover:text-gray-500',
                    'mr-3 h-5 w-5 flex-shrink-0'
                  )}
                />
                {item.name}
              </Link>
            ))}
          </nav>

          {/* Footer */}
          <div className="border-t border-gray-200 p-4">
            <div className="text-xs text-gray-500">
              <div className="mb-1 font-medium">Quản trị Y tế</div>
              <div>Phiên bản 1.0.0</div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Sidebar;
