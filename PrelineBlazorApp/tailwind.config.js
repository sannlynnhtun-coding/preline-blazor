/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Components/Pages/**/*.razor",
        "./Components/Shared/**/*.razor",
        "./Components/Layout/**/*.razor",
        "./wwwroot/**/*.html",
        'node_modules/preline/dist/*.js',
    ],
    darkMode: 'class',
    theme: {
        extend: {},
    },
    plugins: [
        require('preline/plugin'),
    ],
}

