import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'
import { defineConfig, globalIgnores } from 'eslint/config'

// تمپلیت سازمانی به پلاگین «custom» ارجاع می‌دهد که منتشر نشده است و باعث خطای
// «Plugin "plugin:custom" not found» در کل پروژه می‌شود؛ قواعد آن (no-console-log و no-import-mui)
// با قواعد استاندارد زیر جایگزین شده‌اند. vendor و خروجی تولیدشده بررسی نمی‌شوند.
export default defineConfig([
  globalIgnores(['dist', 'vendor', 'src/types/*.ts', 'src/server/services/*.ts']),
  {
    files: ['**/*.{ts,tsx}'],
    extends: [
      js.configs.recommended,
      tseslint.configs.recommended,
      reactHooks.configs['recommended-latest'],
      reactRefresh.configs.vite,
    ],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    rules: {
      'no-console': ['error', { allow: ['warn', 'error'] }],
      // فقط کامپوننت‌های Ikco* استفاده شوند (قانون «No custom styling» استاندارد IKCO)؛ آیکون‌ها مجازند
      'no-restricted-imports': [
        'error',
        {
          paths: [
            { name: '@mui/material', message: 'به جای MUI از کامپوننت‌های @ikco/frontend-kit استفاده کنید.' },
            { name: '@mui/x-data-grid', message: 'به جای MUI از IkcoDataGrid استفاده کنید.' },
            { name: '@mui/x-date-pickers', message: 'به جای MUI از IkcoDatePicker استفاده کنید.' },
          ],
        },
      ],
    },
  },
])
