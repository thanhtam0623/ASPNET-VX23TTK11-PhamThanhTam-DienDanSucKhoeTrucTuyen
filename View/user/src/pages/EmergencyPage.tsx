import React from 'react';
import { Link } from 'react-router-dom';
import { ExclamationTriangleIcon, PhoneIcon } from '@heroicons/react/24/outline';
  import { usePageTitle } from '../utils';

const EmergencyPage: React.FC = () => {
  usePageTitle('Tài nguyên cấp cứu y tế');
  return (
    <div className="mx-auto max-w-4xl px-4 py-8 sm:px-6 lg:px-8">
      <div className="rounded-lg border border-red-200 bg-red-50 p-8 text-center">
        <ExclamationTriangleIcon className="mx-auto h-16 w-16 text-red-600" />
        <h1 className="mt-4 text-3xl font-bold text-red-900">
          Tài nguyên cấp cứu y tế
        </h1>
        <p className="mt-4 text-lg text-red-800">
          Nếu bạn đang gặp tình huống cấp cứu y tế, vui lòng liên hệ dịch vụ cấp cứu ngay lập tức.
        </p>
        
        <div className="mt-8 space-y-4">
          <div className="rounded-lg bg-white p-4 shadow-sm">
            <PhoneIcon className="mx-auto h-8 w-8 text-red-600" />
            <h2 className="mt-2 text-xl font-semibold text-gray-900">Số điện thoại cấp cứu</h2>
            <p className="mt-2 text-gray-700">
              Việt Nam: 115 | Mỹ: 911 | Anh: 999 | EU: 112
            </p>
          </div>
          
          <div className="mt-8">
            <Link
              to="/"
              className="inline-flex items-center rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
            >
              Về cộng đồng
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EmergencyPage;
