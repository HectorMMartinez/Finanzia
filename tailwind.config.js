/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['Prestamo.Web/Views/**/*.cshtml', // Rutas para Razor Pages
    'Prestamo.Web/wwwroot/views/*.js'    // Archivos JS donde uses clases Tailwind
    ],
  theme: {
    extend: {},
  },
  plugins: [],
}

