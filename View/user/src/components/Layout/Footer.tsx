import React from 'react';
import { Link } from 'react-router-dom';

const Footer: React.FC = () => {
  const currentYear = new Date().getFullYear();

  const footerLinks = {
    company: [
      { name: 'Về chúng tôi', href: '/about' },
      { name: 'Tuyên bố y khoa', href: '/medical-disclaimer' },
      { name: 'Chính sách bảo mật', href: '/privacy' },
      { name: 'Điều khoản dịch vụ', href: '/terms' },
    ],
    community: [
      { name: 'Quy tắc cộng đồng', href: '/guidelines' },
      { name: 'Tài nguyên sức khỏe', href: '/resources' },
      { name: 'Liên hệ khẩn cấp', href: '/emergency' },
      { name: 'Báo cáo nội dung', href: '/report' },
    ],
    support: [
      { name: 'Trung tâm trợ giúp', href: '/help' },
      { name: 'Liên hệ đội ngũ y tế', href: '/medical-contact' },
      { name: 'Khả năng tiếp cận', href: '/accessibility' },
      { name: 'Trạng thái hệ thống', href: '/status' },
    ],
  };

  return (
    <footer className="border-t border-gray-200 bg-white">
      <div className="mx-auto max-w-7xl px-4 py-12 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 gap-8 md:grid-cols-4">
          {/* Logo and Description */}
          <div className="col-span-1 md:col-span-1">
            <div className="mb-4 flex items-center space-x-3">
              <div className="rounded-xl bg-gradient-to-br from-primary-600 to-secondary-600 p-2.5 text-white shadow-lg">
                <svg
                  className="h-6 w-6"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path d="M19.5 3A2.5 2.5 0 0122 5.5v13a2.5 2.5 0 01-2.5 2.5h-15A2.5 2.5 0 012 18.5v-13A2.5 2.5 0 014.5 3h15zM12 7a1 1 0 00-1 1v8a1 1 0 002 0V8a1 1 0 00-1-1zm3 2a1 1 0 00-1 1v6a1 1 0 002 0v-6a1 1 0 00-1-1zM9 11a1 1 0 00-1 1v4a1 1 0 002 0v-4a1 1 0 00-1-1z" />
                </svg>
              </div>
              <div className="flex flex-col">
                <span className="text-xl font-bold text-gray-900">
                  Cộng đồng Sức khỏe
                </span>
                <span className="text-xs font-medium text-gray-500">
                  Thông tin Y tế Đáng tin cậy
                </span>
              </div>
            </div>
            <p className="mb-4 text-sm text-gray-600">
              Một cộng đồng y tế đáng tin cậy nơi bệnh nhân, người chăm sóc, và
              các chuyên gia y tế kết nối để chia sẻ kinh nghiệm, tìm kiếm lời khuyên,
              và hỗ trợ lẫn nhau trong hành trình chăm sóc sức khỏe.
            </p>
            <div className="flex space-x-4">
              <a
                href="https://twitter.com"
                target="_blank"
                rel="noopener noreferrer"
                className="text-gray-400 transition-colors duration-200 hover:text-gray-600"
              >
                <span className="sr-only">Twitter</span>
                <svg
                  className="h-5 w-5"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path d="M6.29 18.251c7.547 0 11.675-6.253 11.675-11.675 0-.178 0-.355-.012-.53A8.348 8.348 0 0020 3.92a8.19 8.19 0 01-2.357.646 4.118 4.118 0 001.804-2.27 8.224 8.224 0 01-2.605.996 4.107 4.107 0 00-6.993 3.743 11.65 11.65 0 01-8.457-4.287 4.106 4.106 0 001.27 5.477A4.073 4.073 0 01.8 7.713v.052a4.105 4.105 0 003.292 4.022 4.095 4.095 0 01-1.853.07 4.108 4.108 0 003.834 2.85A8.233 8.233 0 010 16.407a11.616 11.616 0 006.29 1.84" />
                </svg>
              </a>
              <a
                href="https://github.com"
                target="_blank"
                rel="noopener noreferrer"
                className="text-gray-400 transition-colors duration-200 hover:text-gray-600"
              >
                <span className="sr-only">GitHub</span>
                <svg
                  className="h-5 w-5"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path
                    fillRule="evenodd"
                    d="M10 0C4.477 0 0 4.484 0 10.017c0 4.425 2.865 8.18 6.839 9.504.5.092.682-.217.682-.483 0-.237-.008-.868-.013-1.703-2.782.605-3.369-1.343-3.369-1.343-.454-1.158-1.11-1.466-1.11-1.466-.908-.62.069-.608.069-.608 1.003.07 1.531 1.032 1.531 1.032.892 1.53 2.341 1.088 2.91.832.092-.647.35-1.088.636-1.338-2.22-.253-4.555-1.113-4.555-4.951 0-1.093.39-1.988 1.029-2.688-.103-.253-.446-1.272.098-2.65 0 0 .84-.27 2.75 1.026A9.564 9.564 0 0110 4.844c.85.004 1.705.115 2.504.337 1.909-1.296 2.747-1.027 2.747-1.027.546 1.379.203 2.398.1 2.651.64.7 1.028 1.595 1.028 2.688 0 3.848-2.339 4.695-4.566 4.942.359.31.678.921.678 1.856 0 1.338-.012 2.419-.012 2.747 0 .268.18.58.688.482A10.019 10.019 0 0020 10.017C20 4.484 15.522 0 10 0z"
                    clipRule="evenodd"
                  />
                </svg>
              </a>
              <a
                href="https://discord.com"
                target="_blank"
                rel="noopener noreferrer"
                className="text-gray-400 transition-colors duration-200 hover:text-gray-600"
              >
                <span className="sr-only">Discord</span>
                <svg
                  className="h-5 w-5"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path d="M16.942 3.063c-1.415-.647-2.933-1.12-4.526-1.384a.078.078 0 00-.083.037c-.195.347-.412.799-.563 1.156-1.709-.256-3.411-.256-5.088 0-.151-.357-.375-.809-.563-1.156a.081.081 0 00-.083-.037A13.085 13.085 0 001.508 3.063a.073.073 0 00-.034.029C.221 6.348-.132 9.565.041 12.748a.088.088 0 00.034.06 13.205 13.205 0 003.974 2.007.081.081 0 00.088-.029c.486-.663.917-1.36 1.288-2.094a.08.08 0 00-.044-.111 8.69 8.69 0 01-1.249-.595.081.081 0 01-.008-.135c.084-.063.168-.128.248-.194a.078.078 0 01.081-.011c2.62 1.196 5.456 1.196 8.047 0a.078.078 0 01.083.011c.08.066.164.131.248.194a.081.081 0 01-.006.135 8.153 8.153 0 01-1.25.595.08.08 0 00-.043.111c.378.734.809 1.431 1.287 2.094a.08.08 0 00.088.029 13.165 13.165 0 003.974-2.007.081.081 0 00.034-.06c.204-3.678-.341-6.873-1.445-9.714a.063.063 0 00-.033-.029zM6.678 10.865c-.962 0-1.754-.883-1.754-1.967 0-1.084.776-1.967 1.754-1.967.986 0 1.77.891 1.754 1.967 0 1.084-.776 1.967-1.754 1.967zm6.644 0c-.962 0-1.754-.883-1.754-1.967 0-1.084.776-1.967 1.754-1.967.986 0 1.77.891 1.754 1.967 0 1.084-.768 1.967-1.754 1.967z" />
                </svg>
              </a>
            </div>
          </div>

          {/* Company Links */}
          <div>
            <h3 className="mb-4 text-sm font-semibold uppercase tracking-wider text-gray-900">
              Giới thiệu
            </h3>
            <ul className="space-y-3">
              {footerLinks.company.map(link => (
                <li key={link.name}>
                  <Link
                    to={link.href}
                    className="text-sm text-gray-600 transition-colors duration-200 hover:text-gray-900"
                  >
                    {link.name}
                  </Link>
                </li>
              ))}
            </ul>
          </div>

          {/* Community Links */}
          <div>
            <h3 className="mb-4 text-sm font-semibold uppercase tracking-wider text-gray-900">
              Cộng đồng
            </h3>
            <ul className="space-y-3">
              {footerLinks.community.map(link => (
                <li key={link.name}>
                  <Link
                    to={link.href}
                    className="text-sm text-gray-600 transition-colors duration-200 hover:text-gray-900"
                  >
                    {link.name}
                  </Link>
                </li>
              ))}
            </ul>
          </div>

          {/* Support Links */}
          <div>
            <h3 className="mb-4 text-sm font-semibold uppercase tracking-wider text-gray-900">
              Hỗ trợ
            </h3>
            <ul className="space-y-3">
              {footerLinks.support.map(link => (
                <li key={link.name}>
                  {link.href.startsWith('http') ? (
                    <a
                      href={link.href}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="text-sm text-gray-600 transition-colors duration-200 hover:text-gray-900"
                    >
                      {link.name}
                    </a>
                  ) : (
                    <Link
                      to={link.href}
                      className="text-sm text-gray-600 transition-colors duration-200 hover:text-gray-900"
                    >
                      {link.name}
                    </Link>
                  )}
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="mt-8 border-t border-gray-200 pt-8">
          <div className="flex flex-col items-center justify-between md:flex-row">
            <p className="text-sm text-gray-500">
              © {currentYear} Phạm Thanh Tâm
            </p>
            <div className="mt-4 flex space-x-6 md:mt-0">
              <Link
                to="/cookies"
                className="text-sm text-gray-500 transition-colors duration-200 hover:text-gray-600"
              >
                Chính sách Cookie
              </Link>
              <Link
                to="/privacy"
                className="text-sm text-gray-500 transition-colors duration-200 hover:text-gray-600"
              >
                Bảo mật
              </Link>
              <Link
                to="/terms"
                className="text-sm text-gray-500 transition-colors duration-200 hover:text-gray-600"
              >
                Điều khoản
              </Link>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
