import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PlusIcon, XMarkIcon, ChatBubbleLeftRightIcon } from '@heroicons/react/24/outline';
import { topicApi, categoryApi } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { usePageTitle } from '../utils';
import type { CreateTopicRequest, Category } from '../types';

const CreateTopicPage: React.FC = () => {
  usePageTitle('Đặt câu hỏi sức khỏe');
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();
  
  const [formData, setFormData] = useState<CreateTopicRequest>({
    title: '',
    content: '',
    categoryId: 0,
    tags: [],
  });
  const [categories, setCategories] = useState<Category[]>([]);
  const [tagInput, setTagInput] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingCategories, setIsLoadingCategories] = useState(true);
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }
    fetchCategories();
  }, [isAuthenticated, navigate]);

  const fetchCategories = async () => {
    try {
      setIsLoadingCategories(true);
      const response = await categoryApi.getAllCategories();
      if (response.success) {
        setCategories(response.data);
      }
    } catch (err) {
      // Handle categories loading error
    } finally {
      setIsLoadingCategories(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'categoryId' ? parseInt(value) : value,
    }));
    
    // Clear specific error
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: '',
      }));
    }
  };

  const handleAddTag = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' || e.key === ',') {
      e.preventDefault();
      const tag = tagInput.trim().toLowerCase();
      if (tag && !formData.tags.includes(tag) && formData.tags.length < 5) {
        setFormData(prev => ({
          ...prev,
          tags: [...prev.tags, tag],
        }));
        setTagInput('');
      }
    }
  };

  const handleRemoveTag = (tagToRemove: string) => {
    setFormData(prev => ({
      ...prev,
      tags: prev.tags.filter(tag => tag !== tagToRemove),
    }));
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.title.trim()) {
      newErrors.title = 'Tiêu đề là bắt buộc';
    } else if (formData.title.length < 10) {
      newErrors.title = 'Tiêu đề phải có ít nhất 10 ký tự';
    } else if (formData.title.length > 200) {
      newErrors.title = 'Tiêu đề phải ít hơn 200 ký tự';
    }

    if (!formData.content.trim()) {
      newErrors.content = 'Nội dung là bắt buộc';
    } else if (formData.content.length < 50) {
      newErrors.content = 'Nội dung phải có ít nhất 50 ký tự';
    }

    if (!formData.categoryId || formData.categoryId === 0) {
      newErrors.categoryId = 'Vui lòng chọn danh mục';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      setIsLoading(true);
      const response = await topicApi.createTopic(formData);
      if (response.success) {
        navigate(`/topics/${response.data.slug}`);
      } else {
        setErrors({ general: response.message });
      }
    } catch (err: any) {
      setErrors({ general: err.message || 'Không thể tạo chủ đề' });
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoadingCategories) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="h-12 w-12 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-4xl px-4 py-8 sm:px-6 lg:px-8">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-3">
          <ChatBubbleLeftRightIcon className="h-8 w-8 text-primary-600" />
          Đặt câu hỏi sức khỏe
        </h1>
        <p className="mt-2 text-gray-600">
          Chia sẻ mối lo ngại sức khỏe, triệu chứng hoặc câu hỏi với cộng đồng hỗ trợ của chúng tôi. 
          Thành viên của chúng tôi bao gồm các chuyên gia y tế, bệnh nhân và người chăm sóc sẵn sàng giúp đỡ.
        </p>
        
        {/* Guidelines */}
        <div className="mt-4 rounded-lg bg-blue-50 p-4">
          <h3 className="mb-2 text-sm font-semibold text-blue-900 flex items-center gap-2">
            <PlusIcon className="h-4 w-4" />
            Hướng dẫn cộng đồng
          </h3>
          <ul className="space-y-1 text-sm text-blue-800">
            <li>• Hãy cụ thể về triệu chứng hoặc mối lo ngại của bạn</li>
            <li>• Tôn trọng sự riêng tư - tránh chia sẻ thông tin cá nhân</li>
            <li>• Ghi nhớ: Điều này không thay thế lời khuyên y tế chuyên nghiệp</li>
            <li>• Hãy hỗ trợ và tử tế với các thành viên khác trong cộng đồng</li>
          </ul>
        </div>
      </div>

      {/* Form */}
      <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
        <form onSubmit={handleSubmit} className="space-y-6">
          {/* Title */}
          <div>
            <label htmlFor="title" className="block text-sm font-medium text-gray-700">
              Tiêu đề câu hỏi *
            </label>
            <input
              type="text"
              id="title"
              name="title"
              value={formData.title}
              onChange={handleInputChange}
              placeholder="ví dụ: Đau ngực sau khi tập thể dục - tôi có nên lo lắng không?"
              className="mt-1 block w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
              maxLength={200}
            />
            {errors.title && (
              <p className="mt-1 text-sm text-red-600">{errors.title}</p>
            )}
            <p className="mt-1 text-sm text-gray-500">
              {formData.title.length}/200 ký tự
            </p>
          </div>

          {/* Category */}
          <div>
            <label htmlFor="categoryId" className="block text-sm font-medium text-gray-700">
              Danh mục sức khỏe *
            </label>
            <select
              id="categoryId"
              name="categoryId"
              value={formData.categoryId}
              onChange={handleInputChange}
              className="mt-1 block w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value={0}>Chọn danh mục</option>
              {categories.map(category => (
                <option key={category.id} value={category.id}>
                  {category.name} - {category.description}
                </option>
              ))}
            </select>
            {errors.categoryId && (
              <p className="mt-1 text-sm text-red-600">{errors.categoryId}</p>
            )}
          </div>

          {/* Content */}
          <div>
            <label htmlFor="content" className="block text-sm font-medium text-gray-700">
              Mô tả câu hỏi của bạn *
            </label>
            <textarea
              id="content"
              name="content"
              value={formData.content}
              onChange={handleInputChange}
              rows={10}
              placeholder="Vui lòng cung cấp thông tin chi tiết về:
- Triệu chứng hoặc mối lo ngại của bạn
- Bạn đã trải qua điều này trong bao lâu
- Tiền sử bệnh liên quan
- Những gì bạn đã thử
- Câu hỏi cụ thể của bạn

Hãy nhớ tránh chia sẻ thông tin cá nhân có thể nhận dạng."
              className="mt-1 block w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            />
            {errors.content && (
              <p className="mt-1 text-sm text-red-600">{errors.content}</p>
            )}
            <p className="mt-1 text-sm text-gray-500">
              {formData.content.length} ký tự (tối thiểu 50)
            </p>
          </div>

          {/* Tags */}
          <div>
            <label htmlFor="tags" className="block text-sm font-medium text-gray-700">
              Thẻ (Tùy chọn)
            </label>
            <input
              type="text"
              id="tags"
              value={tagInput}
              onChange={(e) => setTagInput(e.target.value)}
              onKeyDown={handleAddTag}
              placeholder="Thêm thẻ như 'đau-ngực', 'tiểu-đường', 'lo-âu' (nhấn Enter hoặc dấu phẩy để thêm)"
              className="mt-1 block w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
              disabled={formData.tags.length >= 5}
            />
            <p className="mt-1 text-sm text-gray-500">
              Nhấn Enter hoặc dấu phẩy để thêm thẻ. Tối đa 5 thẻ.
            </p>
            
            {/* Display Tags */}
            {formData.tags.length > 0 && (
              <div className="mt-2 flex flex-wrap gap-2">
                {formData.tags.map((tag, index) => (
                  <span
                    key={index}
                    className="inline-flex items-center rounded-full bg-primary-100 px-3 py-1 text-sm font-medium text-primary-800"
                  >
                    {tag}
                    <button
                      type="button"
                      onClick={() => handleRemoveTag(tag)}
                      className="ml-1 inline-flex h-4 w-4 items-center justify-center rounded-full text-primary-600 hover:bg-primary-200 hover:text-primary-800"
                    >
                      <XMarkIcon className="h-3 w-3" />
                    </button>
                  </span>
                ))}
              </div>
            )}
          </div>

          {/* General Error */}
          {errors.general && (
            <div className="rounded-md bg-red-50 p-4">
              <div className="text-sm text-red-700">{errors.general}</div>
            </div>
          )}

          {/* Emergency Notice */}
          <div className="rounded-lg bg-red-50 p-4">
            <div className="flex">
              <div className="flex-shrink-0">
                <svg className="h-5 w-5 text-red-400" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
                </svg>
              </div>
              <div className="ml-3">
                <h3 className="text-sm font-medium text-red-800">
                  <XMarkIcon className="h-5 w-5" />
                  Thông báo cấp cứu y tế
                </h3>
                <div className="mt-2 text-sm text-red-700">
                  <p>
                    Nếu bạn đang gặp tình huống cấp cứu y tế (đau ngực, khó thở, 
                    chấn thương nặng, v.v.), vui lòng gọi dịch vụ cấp cứu ngay lập tức hoặc đến 
                    phòng cấp cứu gần nhất. Diễn đàn này không dành cho các tình huống y tế khẩn cấp.
                  </p>
                </div>
              </div>
            </div>
          </div>

          {/* Submit Button */}
          <div className="flex justify-end space-x-3">
            <button
              type="button"
              onClick={() => navigate('/')}
              className="rounded-lg border border-gray-300 px-4 py-2 text-gray-700 hover:bg-gray-50"
            >
              Hủy
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="flex items-center rounded-lg bg-primary-600 px-6 py-2 text-white hover:bg-primary-700 disabled:opacity-50"
            >
              {isLoading ? (
                <>
                  <div className="mr-2 h-4 w-4 animate-spin rounded-full border-b-2 border-white"></div>
                  Đang đăng...
                </>
              ) : (
                <>
                  <PlusIcon className="mr-2 h-4 w-4" />
                  Đăng câu hỏi
                </>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateTopicPage;
