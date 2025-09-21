import { useState, useEffect } from 'react';
import { 
  ExclamationTriangleIcon, 
  FunnelIcon, 
  EyeIcon,
  CheckCircleIcon,
  XCircleIcon,
  ClockIcon,
  ArrowPathIcon
} from '@heroicons/react/24/outline';
import { reportsApi } from '../services/api';
import type { Report } from '../types';
import { formatDate } from '../utils';
import { usePageTitle } from '../utils';
import Badge from '../components/UI/Badge';
import Modal from '../components/UI/Modal';

interface ReportDetailModalProps {
  report: Report | null;
  isOpen: boolean;
  onClose: () => void;
  onStatusUpdate: (reportId: number, status: string) => void;
}

const ReportDetailModal: React.FC<ReportDetailModalProps> = ({ 
  report, 
  isOpen, 
  onClose, 
  onStatusUpdate 
}) => {
  const [updating, setUpdating] = useState(false);

  if (!report) return null;

  const handleStatusUpdate = async (status: string) => {
    setUpdating(true);
    try {
      await onStatusUpdate(report.id, status);
      onClose();
    } catch (error) {
      console.error('Failed to update report status:', error);
    } finally {
      setUpdating(false);
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Chờ xử lý': return 'warning';
      case 'Đã xem xét': return 'primary';
      case 'Đã giải quyết': return 'success';
      case 'Đã từ chối': return 'danger';
      default: return 'secondary';
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Chi tiết báo cáo">
      <div className="space-y-6">
        {/* Report Header */}
        <div className="flex items-start justify-between">
          <div>
            <h3 className="text-lg font-semibold text-gray-900">
Báo cáo #{report.id}
            </h3>
            <p className="text-sm text-gray-600">
Được báo cáo bởi <span className="font-medium">{report.userName}</span>
            </p>
          </div>
          <Badge color={getStatusColor(report.status)} variant="filled">
            {report.status}
          </Badge>
        </div>

        {/* Report Details */}
        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Danh mục
            </label>
            <p className="text-sm text-gray-900">{report.category}</p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Lý do
            </label>
            <p className="text-sm text-gray-900">{report.reason}</p>
          </div>

          {report.topicTitle && (
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Topic Title
              </label>
              <p className="text-sm text-gray-900">{report.topicTitle}</p>
            </div>
          )}

          {report.postContent && (
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Post Content
              </label>
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-900 line-clamp-4">
                  {report.postContent}
                </p>
              </div>
            </div>
          )}

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Reported At
              </label>
              <p className="text-sm text-gray-900">{formatDate(report.createdAt)}</p>
            </div>
            {report.reviewedAt && (
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Reviewed At
                </label>
                <p className="text-sm text-gray-900">{formatDate(report.reviewedAt)}</p>
              </div>
            )}
          </div>

          {report.reviewedByAdmin && (
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Reviewed By
              </label>
              <p className="text-sm text-gray-900">{report.reviewedByAdmin}</p>
            </div>
          )}
        </div>

        {/* Action Buttons */}
        {report.status === 'Chờ xử lý' && (
          <div className="flex space-x-3 pt-4 border-t">
            <button
              onClick={() => handleStatusUpdate('Đã xem xét')}
              disabled={updating}
              className="flex-1 bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 flex items-center justify-center"
            >
              {updating ? (
                <ArrowPathIcon className="h-4 w-4 animate-spin" />
              ) : (
                <>
                  <EyeIcon className="h-4 w-4 mr-2" />
                  Mark as Reviewed
                </>
              )}
            </button>
            <button
              onClick={() => handleStatusUpdate('Đã giải quyết')}
              disabled={updating}
              className="flex-1 bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 disabled:opacity-50 flex items-center justify-center"
            >
              {updating ? (
                <ArrowPathIcon className="h-4 w-4 animate-spin" />
              ) : (
                <>
                  <CheckCircleIcon className="h-4 w-4 mr-2" />
                  Resolve
                </>
              )}
            </button>
            <button
              onClick={() => handleStatusUpdate('Đã từ chối')}
              disabled={updating}
              className="flex-1 bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700 disabled:opacity-50 flex items-center justify-center"
            >
              {updating ? (
                <ArrowPathIcon className="h-4 w-4 animate-spin" />
              ) : (
                <>
                  <XCircleIcon className="h-4 w-4 mr-2" />
                  Dismiss
                </>
              )}
            </button>
          </div>
        )}
      </div>
    </Modal>
  );
};

const ReportsPage: React.FC = () => {
  usePageTitle('Quản lý báo cáo');
  
  const [reports, setReports] = useState<Report[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [statusFilter, setStatusFilter] = useState<string>('');
  const [selectedReport, setSelectedReport] = useState<Report | null>(null);
  const [showDetailModal, setShowDetailModal] = useState(false);

  const pageSize = 20;

  const fetchReports = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await reportsApi.getReports(
        currentPage,
        pageSize,
        statusFilter || undefined
      );
      
      if (response.success && response.data) {
        setReports(response.data.items);
        setTotalPages(response.data.totalPages);
      } else {
        setError(response.message || 'Failed to fetch reports');
      }
    } catch (err: any) {
      console.error('Error fetching reports:', err);
      setError(err.message || 'Failed to fetch reports');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchReports();
  }, [currentPage, statusFilter]);

  const handleStatusUpdate = async (reportId: number, status: string) => {
    try {
      const response = await reportsApi.updateReportStatus(reportId, { status } as any);
      
      if (response.success) {
        // Refresh the reports list
        await fetchReports();
      } else {
        throw new Error(response.message || 'Failed to update status');
      }
    } catch (err: any) {
      console.error('Error updating report status:', err);
      throw err;
    }
  };

  const handleViewReport = (report: Report) => {
    setSelectedReport(report);
    setShowDetailModal(true);
  };

  const getStatusBadgeColor = (status: string) => {
    switch (status) {
      case 'Chờ xử lý': return 'warning';
      case 'Đã xem xét': return 'primary';
      case 'Đã giải quyết': return 'success';
      case 'Đã từ chối': return 'danger';
      default: return 'secondary';
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Chờ xử lý': return <ClockIcon className="h-4 w-4" />;
      case 'Đã xem xét': return <EyeIcon className="h-4 w-4" />;
      case 'Đã giải quyết': return <CheckCircleIcon className="h-4 w-4" />;
      case 'Đã từ chối': return <XCircleIcon className="h-4 w-4" />;
      default: return <ClockIcon className="h-4 w-4" />;
    }
  };

  const getReportCounts = () => {
    return {
      total: reports.length,
      pending: reports.filter(r => r.status === 'Chờ xử lý').length,
      reviewed: reports.filter(r => r.status === 'Đã xem xét').length,
      resolved: reports.filter(r => r.status === 'Đã giải quyết').length,
      dismissed: reports.filter(r => r.status === 'Đã từ chối').length,
    };
  };

  const counts = getReportCounts();

  if (loading && reports.length === 0) {
    return (
      <div className="p-6">
        <div className="flex items-center justify-center h-64">
          <ArrowPathIcon className="h-8 w-8 animate-spin text-primary-600" />
        </div>
      </div>
    );
  }

  return (
    <div className="p-6">
      {/* Header */}
      <div className="mb-6">
        <div className="flex items-center mb-4">
          <ExclamationTriangleIcon className="h-8 w-8 text-orange-600 mr-3" />
          <div>
            <h1 className="text-2xl font-bold text-gray-900">Quản lý Báo cáo</h1>
            <p className="text-gray-600">Xem xét và quản lý báo cáo từ người dùng</p>
          </div>
        </div>

        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-5 gap-4 mb-6">
          <div className="bg-white p-4 rounded-lg border">
            <div className="flex items-center">
              <div className="p-2 bg-gray-100 rounded-lg">
                <ExclamationTriangleIcon className="h-6 w-6 text-gray-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Tổng Báo cáo</p>
                <p className="text-2xl font-bold text-gray-900">{counts.total}</p>
              </div>
            </div>
          </div>
          <div className="bg-white p-4 rounded-lg border">
            <div className="flex items-center">
              <div className="p-2 bg-orange-100 rounded-lg">
                <ClockIcon className="h-6 w-6 text-orange-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Chờ xử lý</p>
                <p className="text-2xl font-bold text-orange-600">{counts.pending}</p>
              </div>
            </div>
          </div>
          <div className="bg-white p-4 rounded-lg border">
            <div className="flex items-center">
              <div className="p-2 bg-blue-100 rounded-lg">
                <EyeIcon className="h-6 w-6 text-blue-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Đã xem xét</p>
                <p className="text-2xl font-bold text-blue-600">{counts.reviewed}</p>
              </div>
            </div>
          </div>
          <div className="bg-white p-4 rounded-lg border">
            <div className="flex items-center">
              <div className="p-2 bg-green-100 rounded-lg">
                <CheckCircleIcon className="h-6 w-6 text-green-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Đã giải quyết</p>
                <p className="text-2xl font-bold text-green-600">{counts.resolved}</p>
              </div>
            </div>
          </div>
          <div className="bg-white p-4 rounded-lg border">
            <div className="flex items-center">
              <div className="p-2 bg-red-100 rounded-lg">
                <XCircleIcon className="h-6 w-6 text-red-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Đã từ chối</p>
                <p className="text-2xl font-bold text-red-600">{counts.dismissed}</p>
              </div>
            </div>
          </div>
        </div>

        {/* Filters */}
        <div className="flex items-center space-x-4">
          <div className="flex items-center">
            <FunnelIcon className="h-5 w-5 text-gray-400 mr-2" />
            <select
              value={statusFilter}
              onChange={(e) => {
                setStatusFilter(e.target.value);
                setCurrentPage(1);
              }}
              className="border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary-500 focus:border-transparent"
            >
              <option value="">Tất cả trạng thái</option>
              <option value="Pending">Pending</option>
              <option value="Reviewed">Reviewed</option>
              <option value="Resolved">Resolved</option>
              <option value="Dismissed">Dismissed</option>
            </select>
          </div>
          <button
            onClick={fetchReports}
            disabled={loading}
            className="btn-secondary flex items-center"
          >
            <ArrowPathIcon className={`h-4 w-4 mr-2 ${loading ? 'animate-spin' : ''}`} />
Làm mới
          </button>
        </div>
      </div>

      {error && (
        <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4">
          <div className="flex">
            <XCircleIcon className="h-5 w-5 text-red-400" />
            <div className="ml-3">
              <p className="text-sm text-red-700">{error}</p>
            </div>
          </div>
        </div>
      )}

      {/* Reports Table */}
      <div className="bg-white rounded-lg border overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Report ID
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Reporter
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Category
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Reason
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Status
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Reported At
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {reports.map((report) => (
                <tr key={report.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    #{report.id}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900 font-medium">{report.userName}</div>
                    {report.topicTitle && (
                      <div className="text-sm text-gray-500 truncate max-w-xs">
                        Topic: {report.topicTitle}
                      </div>
                    )}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {report.category}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900 max-w-xs truncate">
                      {report.reason}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <Badge color={getStatusBadgeColor(report.status)} variant="filled">
                      <div className="flex items-center">
                        {getStatusIcon(report.status)}
                        <span className="ml-1">{report.status}</span>
                      </div>
                    </Badge>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {formatDate(report.createdAt)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                    <button
                      onClick={() => handleViewReport(report)}
                      className="text-primary-600 hover:text-primary-900 flex items-center"
                    >
                      <EyeIcon className="h-4 w-4 mr-1" />
                      View
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="mt-6 flex items-center justify-between">
          <div className="text-sm text-gray-700">
            Page {currentPage} of {totalPages}
          </div>
          <div className="flex space-x-2">
            <button
              onClick={() => setCurrentPage(Math.max(1, currentPage - 1))}
              disabled={currentPage === 1}
              className="btn-secondary disabled:opacity-50"
            >
              Previous
            </button>
            <button
              onClick={() => setCurrentPage(Math.min(totalPages, currentPage + 1))}
              disabled={currentPage === totalPages}
              className="btn-secondary disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {/* Empty State */}
      {!loading && reports.length === 0 && (
        <div className="text-center py-12">
          <ExclamationTriangleIcon className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-medium text-gray-900">No reports found</h3>
          <p className="mt-1 text-sm text-gray-500">
            {statusFilter ? `No reports with status "${statusFilter}"` : 'No reports have been submitted yet'}
          </p>
        </div>
      )}

      {/* Report Detail Modal */}
      <ReportDetailModal
        report={selectedReport}
        isOpen={showDetailModal}
        onClose={() => {
          setShowDetailModal(false);
          setSelectedReport(null);
        }}
        onStatusUpdate={handleStatusUpdate}
      />
    </div>
  );
};

export default ReportsPage;
