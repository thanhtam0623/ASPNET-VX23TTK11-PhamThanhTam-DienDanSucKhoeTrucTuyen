import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  UsersIcon,
  DocumentTextIcon,
  ChatBubbleLeftRightIcon,
  FolderIcon,
  ArrowTrendingUpIcon,
} from '@heroicons/react/24/outline';
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  AreaChart,
  Area,
} from 'recharts';
import { dashboardApi } from '../services/api';
import type { DashboardData } from '../types';
import { formatNumber, getChartColor, usePageTitle } from '../utils';

const DashboardPage: React.FC = () => {
  usePageTitle('Bảng điều khiển');
  
  const navigate = useNavigate();
  const [dashboardData, setDashboardData] = useState<DashboardData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    try {
      setIsLoading(true);
      const response = await dashboardApi.getDashboard();
      if (response.success) {
        setDashboardData(response.data);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Failed to load dashboard data');
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-md bg-red-50 p-4">
        <div className="text-sm text-red-700">{error}</div>
      </div>
    );
  }

  if (!dashboardData) {
    return null;
  }

  const { stats, chartData, quickActions } = dashboardData;
  const { usersChart, topicsChart } = chartData;

  const statCards = [
    {
      title: 'Tổng người dùng',
      value: stats.totalUsers,
      change: stats.newUsersToday,
      changeText: 'hôm nay',
      icon: UsersIcon,
      color: 'text-blue-600',
      bgColor: 'bg-blue-100',
    },
    {
      title: 'Tổng chủ đề',
      value: stats.totalTopics,
      change: stats.newTopicsToday,
      changeText: 'hôm nay',
      icon: DocumentTextIcon,
      color: 'text-green-600',
      bgColor: 'bg-green-100',
    },
    {
      title: 'Tổng bài viết',
      value: stats.totalPosts,
      change: stats.newPostsToday,
      changeText: 'hôm nay',
      icon: ChatBubbleLeftRightIcon,
      color: 'text-purple-600',
      bgColor: 'bg-purple-100',
    },
    {
      title: 'Danh mục',
      value: stats.totalCategories || 0,
      change: 0,
      changeText: 'tổng',
      icon: FolderIcon,
      color: 'text-orange-600',
      bgColor: 'bg-orange-100',
    },
  ];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Bảng điều khiển</h1>
        <p className="mt-1 text-sm text-gray-500">
          Tổng quan nền tảng cộng đồng y tế của bạn
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        {statCards.map((stat, index) => (
          <div key={index} className="card">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className={`rounded-lg p-3 ${stat.bgColor}`}>
                  <stat.icon className={`h-6 w-6 ${stat.color}`} />
                </div>
              </div>
              <div className="ml-5 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">
                    {stat.title}
                  </dt>
                  <dd className="flex items-baseline">
                    <div className="text-2xl font-semibold text-gray-900">
                      {formatNumber(stat.value)}
                    </div>
                    {stat.change > 0 && (
                      <div className="ml-2 flex items-baseline text-sm font-semibold text-green-600">
                        <ArrowTrendingUpIcon className="h-4 w-4 mr-1 flex-shrink-0" />
                        +{stat.change} {stat.changeText}
                      </div>
                    )}
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        {/* User Growth Chart */}
        <div className="card">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Tăng trưởng người dùng</h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart data={usersChart || []}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis 
                  dataKey="date" 
                  fontSize={12}
                  tickFormatter={(value) => new Date(value).toLocaleDateString()}
                />
                <YAxis fontSize={12} />
                <Tooltip 
                  labelFormatter={(value) => new Date(value).toLocaleDateString()}
                  formatter={(value) => [value, 'Người dùng mới']}
                />
                <Area
                  type="monotone"
                  dataKey="count"
                  stroke={getChartColor(0)}
                  fill={getChartColor(0)}
                  fillOpacity={0.3}
                />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Topic & Post Growth Chart */}
        <div className="card">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Tăng trưởng nội dung</h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={topicsChart || []}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis 
                  dataKey="date" 
                  fontSize={12}
                  tickFormatter={(value) => new Date(value).toLocaleDateString()}
                />
                <YAxis fontSize={12} />
                <Tooltip 
                  labelFormatter={(value) => new Date(value).toLocaleDateString()}
                />
                <Line
                  type="monotone"
                  dataKey="count"
                  stroke={getChartColor(1)}
                  strokeWidth={2}
                  name="Chủ đề"
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="card">
        <h3 className="text-lg font-medium text-gray-900 mb-4">Thao tác nhanh</h3>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {(quickActions || []).map((action, index) => (
            <div
              key={index}
              className="relative group bg-white p-6 focus-within:ring-2 focus-within:ring-inset focus-within:ring-indigo-500 rounded-lg border border-gray-200 hover:border-gray-300"
            >
              <div>
                <span className={`rounded-lg inline-flex p-3 ring-4 ring-white ${
                  action.color === 'primary' ? 'bg-indigo-50 text-indigo-600' :
                  action.color === 'success' ? 'bg-green-50 text-green-600' :
                  action.color === 'info' ? 'bg-blue-50 text-blue-600' :
                  action.color === 'warning' ? 'bg-yellow-50 text-yellow-600' :
                  'bg-gray-50 text-gray-600'
                }`}>
                  {action.icon === 'users' && <UsersIcon className="h-6 w-6" />}
                  {action.icon === 'folder' && <FolderIcon className="h-6 w-6" />}
                  {action.icon === 'message-square' && <DocumentTextIcon className="h-6 w-6" />}
                  {action.icon === 'flag' && <ArrowTrendingUpIcon className="h-6 w-6" />}
                  {action.icon === 'settings' && <ArrowTrendingUpIcon className="h-6 w-6" />}
                </span>
              </div>
              <div className="mt-8">
                <h3 className="text-lg font-medium">
                  <button 
                    onClick={() => navigate(action.url.replace('/admin', ''))} 
                    className="focus:outline-none text-left"
                  >
                    <span className="absolute inset-0" aria-hidden="true" />
                    {action.title}
                  </button>
                </h3>
                <p className="mt-2 text-sm text-gray-500">
                  {action.description}
                </p>
              </div>
              <span
                className="pointer-events-none absolute top-6 right-6 text-gray-300 group-hover:text-gray-400"
                aria-hidden="true"
              >
                <ArrowTrendingUpIcon className="h-6 w-6" />
              </span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
