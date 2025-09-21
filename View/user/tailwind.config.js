/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        // Healthcare Blue - Trust, Medical, Professional
        primary: {
          50: '#eff8ff',
          100: '#dbeefe',
          200: '#bfe4fd',
          300: '#93d5fc',
          400: '#60bdf9',
          500: '#3b9df5',
          600: '#2670ea',
          700: '#1e5fd7',
          800: '#1f4fae',
          900: '#1f4489',
          950: '#172b54',
        },
        // Medical Green - Health, Wellness, Care
        secondary: {
          50: '#f0fdf4',
          100: '#dcfce7',
          200: '#bbf7d0',
          300: '#86efac',
          400: '#4ade80',
          500: '#22c55e',
          600: '#16a34a',
          700: '#15803d',
          800: '#166534',
          900: '#14532d',
          950: '#052e16',
        },
        // Accent colors for healthcare
        accent: {
          orange: '#f97316', // Emergency/Warning
          red: '#ef4444', // Critical/Alert
          purple: '#8b5cf6', // Specialist/Premium
          teal: '#14b8a6', // Wellness/Mental Health
        },
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
      },
      animation: {
        'fade-in': 'fadeIn 0.5s ease-in-out',
        'slide-up': 'slideUp 0.3s ease-out',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%': { transform: 'translateY(20px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
      },
    },
  },
  plugins: [],
};
