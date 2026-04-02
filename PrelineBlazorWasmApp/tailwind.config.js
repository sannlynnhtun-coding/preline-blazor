/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Components/**/*.razor",
    "./**/*.cshtml",
    "./wwwroot/js/**/*.js"
  ],
  darkMode: "class",
  theme: {
    extend: {}
  },
  plugins: []
};
