import React from 'react';
import { Link } from 'react-router-dom';
import { HomeIcon } from '@heroicons/react/24/outline';
import { usePageTitle } from '../utils';

const NotFoundPage: React.FC = () => {
  usePageTitle('Không tìm thấy trang');
  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4 py-12 sm:px-6 lg:px-8">
      <div className="w-full max-w-md text-center">
        <div className="mb-8">
          <h1 className="text-9xl font-bold text-primary-600">404</h1>
          <h2 className="mt-4 text-3xl font-bold text-gray-900">
            Không tìm thấy trang
          </h2>
          <p className="mt-2 text-gray-600">
            Xin lỗi, chúng tôi không thể tìm thấy trang bạn đang tìm kiếm.
          </p>
        </div>

        <div className="space-y-4">
          <Link
            to="/"
            className="inline-flex items-center rounded-lg border border-transparent bg-primary-600 px-4 py-2 text-sm font-medium text-white hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:ring-offset-2"
          >
            <HomeIcon className="mr-2 h-4 w-4" />
            Về trang chủ
          </Link>

          <div className="text-sm text-gray-500">
            <p>
              Hoặc thử{' '}
              <Link
                to="/search"
                className="text-primary-600 hover:text-primary-500"
              >
                tìm kiếm
              </Link>{' '}
              những gì bạn cần.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default NotFoundPage;
