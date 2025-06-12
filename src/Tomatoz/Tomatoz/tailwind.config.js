/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Components/**/*.{razor,html,js}",
    "./Pages/**/*.{razor,html,js}",
    "./Views/**/*.{razor,html,js}",
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        // Thème principal basé sur les tomates et le jardinage
        tomato: {
          50: '#fef7f0',
          100: '#fdede0',
          200: '#fad8c1',
          300: '#f6bd96',
          400: '#f19769',
          500: '#ed7647',
          600: '#de5f38',
          700: '#c04930',
          800: '#9a3c2c',
          900: '#7c3428',
          950: '#431812',
        },
        leaf: {
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
        earth: {
          50: '#faf9f7',
          100: '#f3f0eb',
          200: '#e7ded4',
          300: '#d6c7b5',
          400: '#c0a891',
          500: '#b1937a',
          600: '#a3846f',
          700: '#886e5d',
          800: '#6f5b4f',
          900: '#5a4a42',
          950: '#2e2521',
        },
        sunshine: {
          50: '#fffceb',
          100: '#fff7c7',
          200: '#ffed8a',
          300: '#ffdd4d',
          400: '#ffc820',
          500: '#f9a507',
          600: '#dd7d02',
          700: '#b85806',
          800: '#95440c',
          900: '#7a380d',
          950: '#461c02',
        }
      },
      fontFamily: {
        'sans': ['Inter', 'ui-sans-serif', 'system-ui', 'sans-serif'],
        'display': ['Poppins', 'ui-sans-serif', 'system-ui', 'sans-serif'],
      },
      spacing: {
        '18': '4.5rem',
        '88': '22rem',
        '92': '23rem',
        '96': '24rem',
        '100': '25rem',
        '104': '26rem',
        '108': '27rem',
        '112': '28rem',
        '116': '29rem',
        '120': '30rem',
      },
      animation: {
        'fade-in': 'fadeIn 0.5s ease-in-out',
        'slide-up': 'slideUp 0.3s ease-out',
        'bounce-gentle': 'bounceGentle 2s infinite',
        'pulse-slow': 'pulse 3s infinite',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%': { transform: 'translateY(10px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
        bounceGentle: {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%': { transform: 'translateY(-5px)' },
        }
      },
      backdropBlur: {
        xs: '2px',
      }
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
  ],
}
